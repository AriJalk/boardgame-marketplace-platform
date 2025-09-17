using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingsSold;
using BoardGameMarket.ListingsService.Core.Application.Identity;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingsSold;
using BoardGameMarket.ListingsService.Core.Application.Services.Assemblers.DTOs;
using BoardGameMarket.ListingsService.Core.Application.Services.Helpers;
using BoardGameMarket.ListingsService.Core.Domain.Common;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using BoardGameMarket.ListingsService.Core.Domain.Services;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.External.ListingsSold
{
	internal class CompleteListingSaleUseCase
	{
		private readonly IListingSoldAdderService _listingSoldAdderService;
		private readonly IListingUpdaterService _listingUpdaterService;
		private readonly IListingQueueEntryUpdaterService _listingQueueEntryUpdaterService;
		private readonly SaleDomainService _saleDomainService;
		private readonly ListingFetcher _listingFetcher;
		private readonly ListingValidations _listingValidations;
		private readonly ListingSoldAddRequestComposer _listingSoldAddRequestComposer;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserContext _currentUserContext;

		public CompleteListingSaleUseCase(IListingSoldAdderService listingSoldAdderService, IListingUpdaterService listingUpdaterService, IListingQueueEntryUpdaterService listingQueueEntryUpdaterService, SaleDomainService saleDomainService, ListingFetcher listingFetcher, ListingValidations listingValidations, ListingSoldAddRequestComposer listingSoldAddRequestComposer, IUnitOfWork unitOfWork, ICurrentUserContext currentUserContext)
		{
			_listingSoldAdderService = listingSoldAdderService;
			_listingUpdaterService = listingUpdaterService;
			_listingQueueEntryUpdaterService = listingQueueEntryUpdaterService;
			_saleDomainService = saleDomainService;
			_listingFetcher = listingFetcher;
			_listingValidations = listingValidations;
			_listingSoldAddRequestComposer = listingSoldAddRequestComposer;
			_unitOfWork = unitOfWork;
			_currentUserContext = currentUserContext;
		}

		private async Task<ListingSold> AddListingSoldAsync(Guid listingId, Guid listingQueueEntryId)
		{
			ListingSoldAddRequest listingSoldAddRequest = await _listingSoldAddRequestComposer.ComposeListingSoldAddRequest(listingId, listingQueueEntryId);
			ListingSold? listingSold = await _listingSoldAdderService.AddListingSoldAsync(listingSoldAddRequest.ToListingSold());
			if (listingSold == null) { 
				throw new InvalidOperationException("Can't add sale"); 
			}
			return listingSold;
		}

		private async Task UpdateListingAsync(Listing listing)
		{
			listing.Status = ListingStatus.Pending;
			if (await _listingUpdaterService.UpdateListingAsync(listing) == null)
			{
				throw new InvalidOperationException("Can't update listing");
			}
		}

		private async Task UpdateListingQueueEntryAsync(ListingQueueEntry requestQueueEntry)
		{
			requestQueueEntry.Status = QueueStatus.Approved;
			if (await _listingQueueEntryUpdaterService.UpdateListingQueueEntryAsync(requestQueueEntry) == null)
			{
				throw new InvalidOperationException("Can't update queue entry");
			}
		}

		private void ValidateResult(ListingQueueEntry requestQueueEntry, Listing listing)
		{
			Result canSellResult = _saleDomainService.CanSell(requestQueueEntry, listing);
			if (!canSellResult.IsValid)
			{
				throw new InvalidOperationException(string.Join("\n", canSellResult.Errors));
			}
		}

		public async Task<ListingSold> ExecuteAsync(Guid requestQueueEntryId)
		{
			ListingQueueEntry? requestQueueEntry = await _listingFetcher.GetListingQueueEntryAsync(requestQueueEntryId);
			Guid listingId = requestQueueEntry.ListingId;
			Listing? listing = await _listingFetcher.GetListingAsync(listingId);

			//Authorize
			Guid sellerId = await _listingFetcher.GetSellerUserIdByGroupIdAsync(listing.ListingGroupId);
			IdentityHelper.AuthorizeCurrentUserOrAdmin(sellerId, _currentUserContext);

			await _listingValidations.ValidateListingNotSold(listing);
			ValidateResult(requestQueueEntry, listing);
			try
			{
				await _unitOfWork.BeginTransactionAsync();
				ListingSold listingSold = await AddListingSoldAsync(listingId, requestQueueEntryId);
				await UpdateListingAsync(listing);
				await UpdateListingQueueEntryAsync(requestQueueEntry);
				await _unitOfWork.CommitTransactionAsync();
				return listingSold;
			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}
}
