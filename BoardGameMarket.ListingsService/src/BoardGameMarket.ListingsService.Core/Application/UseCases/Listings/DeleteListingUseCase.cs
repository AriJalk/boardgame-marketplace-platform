using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.Listings
{
	public class DeleteListingUseCase
	{
		private readonly IListingDeleterService _listingDeleterService;
		private readonly IListingItemDeleterService _listingItemDeleterService;
		private readonly IListingQueueEntryDeleterService _listingQueueEntryDeleterService;
		private readonly IUnitOfWork _unitOfWork;

		public DeleteListingUseCase(IListingDeleterService listingDeleterService, IListingItemDeleterService listingItemDeleterService, IListingQueueEntryDeleterService listingQueueEntryDeleterService, IUnitOfWork unitOfWork)
		{
			_listingDeleterService = listingDeleterService;
			_listingItemDeleterService = listingItemDeleterService;
			_listingQueueEntryDeleterService = listingQueueEntryDeleterService;
			_unitOfWork = unitOfWork;
		}

		public async Task<bool> ExecuteAsync(Guid listingId)
		{
			await _unitOfWork.BeginTransactionAsync();
			bool isListingDeleted = await _listingDeleterService.DeleteListingAsync(listingId);
			await _listingItemDeleterService.DeleteListingItemsByListingIdAsync(listingId);
			await _listingQueueEntryDeleterService.DeleteListingQueueEntriesByListingIdAsync(listingId);
			await _unitOfWork.CommitTransactionAsync();
			return isListingDeleted;
		}
	}
}
