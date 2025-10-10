using BoardGameMarket.ListingsService.Core.Application.Identity;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.Services.Helpers;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.Listings
{
	public class DeleteListingUseCase
	{
		private readonly IListingDeleterService _listingDeleterService;
		private readonly IListingItemDeleterService _listingItemDeleterService;
		private readonly IListingQueueEntryDeleterService _listingQueueEntryDeleterService;
		private readonly ListingFetcher _listingFetcher;

		private readonly IUnitOfWork _unitOfWork;
		private readonly IIdentityValidationService _identityValidationService;

		public DeleteListingUseCase(IListingDeleterService listingDeleterService, IListingItemDeleterService listingItemDeleterService, IListingQueueEntryDeleterService listingQueueEntryDeleterService, ListingFetcher listingFetcher, IUnitOfWork unitOfWork, IIdentityValidationService identityValidationService)
		{
			_listingDeleterService = listingDeleterService;
			_listingItemDeleterService = listingItemDeleterService;
			_listingQueueEntryDeleterService = listingQueueEntryDeleterService;
			_listingFetcher = listingFetcher;
			_unitOfWork = unitOfWork;
			_identityValidationService = identityValidationService;
		}

		public async Task<bool> ExecuteAsync(Guid listingId)
		{
			await _unitOfWork.BeginTransactionAsync();

			Guid sellerId = await _listingFetcher.GetSellerUserIdByListingIdAsync(listingId);
			_identityValidationService.ValidateSameUserOrAtLeastRole(sellerId);

			bool isListingDeleted = await _listingDeleterService.DeleteListingAsync(listingId);
			await _listingItemDeleterService.DeleteListingItemsByListingIdAsync(listingId);
			await _listingQueueEntryDeleterService.DeleteListingQueueEntriesByListingIdAsync(listingId);
			await _unitOfWork.CommitTransactionAsync();
			return isListingDeleted;
		}
	}
}
