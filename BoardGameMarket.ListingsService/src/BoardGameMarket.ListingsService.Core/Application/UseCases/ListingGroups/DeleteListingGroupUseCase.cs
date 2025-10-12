using BoardGameMarket.ListingsService.Core.Application.Identity;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.Services.Helpers;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingGroups
{
	public class DeleteListingGroupUseCase
	{
		private readonly IListingDeleterService _listingDeleterService;
		private readonly IListingItemDeleterService _listingItemDeleterService;
		private readonly IListingQueueEntryDeleterService _listingQueueEntryDeleterService;
		private readonly IListingGroupDeleterService _listingGroupDeleterService;
		private readonly IListingGetterService _listingGetterService;
		private readonly ListingFetcher _listingFetcher;

		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserContext _currentUserContext;

		public DeleteListingGroupUseCase(IListingDeleterService listingDeleterService, IListingItemDeleterService listingItemDeleterService, IListingQueueEntryDeleterService listingQueueEntryDeleterService, IListingGroupDeleterService listingGroupDeleterService, IListingGetterService listingGetterService, ListingFetcher listingFetcher, IUnitOfWork unitOfWork, ICurrentUserContext currentUserContext)
		{
			_listingDeleterService = listingDeleterService;
			_listingItemDeleterService = listingItemDeleterService;
			_listingQueueEntryDeleterService = listingQueueEntryDeleterService;
			_listingGroupDeleterService = listingGroupDeleterService;
			_listingGetterService = listingGetterService;
			_listingFetcher = listingFetcher;
			_unitOfWork = unitOfWork;
			_currentUserContext = currentUserContext;
		}

		public async Task<bool> ExecuteAsync(Guid listingGroupId)
		{
			await _unitOfWork.BeginTransactionAsync();
			ListingGroup listingGroup = await _listingFetcher.GetListingGroupByIdAsync(listingGroupId);
			// Validate
			_currentUserContext.ValidateSameUserOrAtLeastRole(listingGroup.SellerUserId, RoleType.Moderator);
			
			IEnumerable<Listing> listings = (await _listingGetterService.GetListingsByListingGroupIdAsync(listingGroupId)).ToList();
			foreach (Listing listing in listings)
			{
				Guid listingId = listing.Id;
				await _listingItemDeleterService.DeleteListingItemsByListingIdAsync(listingId);
				await _listingQueueEntryDeleterService.DeleteListingQueueEntriesByListingIdAsync(listingId);
			}
			await _listingDeleterService.DeleteListingsByListingGroupId(listingGroupId);

			bool isListingGroupDeleted = await _listingGroupDeleterService.DeleteListingGroup(listingGroupId);

			await _unitOfWork.CommitTransactionAsync();
			return isListingGroupDeleted;
		}
	}
}
