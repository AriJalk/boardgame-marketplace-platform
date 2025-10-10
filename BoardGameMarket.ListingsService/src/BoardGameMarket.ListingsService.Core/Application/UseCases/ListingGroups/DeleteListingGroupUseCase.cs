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

		private readonly IUnitOfWork _unitOfWork;
		private readonly IIdentityValidationService _identityValidationService;



		public async Task<bool> ExecuteAsync(Guid listingGroupId)
		{
			await _unitOfWork.BeginTransactionAsync();
			
			
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
