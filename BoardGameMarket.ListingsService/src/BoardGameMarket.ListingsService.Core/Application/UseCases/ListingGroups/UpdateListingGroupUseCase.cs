using BoardGameMarket.ListingsService.Core.Application.Access.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.Contracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Domain.Aggragates;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;


namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingGroups
{
    public class UpdateListingGroupUseCase
    {
        private readonly IListingGroupReadService _listingGroupReadService;
        private readonly IListingGroupWriteService _listingGroupWriteService;
        private readonly IListingReadService _listingReadService;
        private readonly IListingWriteService _listingWriteService;
        private readonly IListingItemReadService _listingItemReadService;
        private readonly IListingItemWriteService _listingItemWriteService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IListingQueueEntryWriteService _listingQueueEntryWriteService;

        public UpdateListingGroupUseCase(IListingGroupReadService listingGroupReadService, IListingGroupWriteService listingGroupWriteService, IListingReadService listingReadService, IListingWriteService listingWriteService, IListingItemReadService listingItemReadService, IListingItemWriteService listingItemWriteService, IUnitOfWork unitOfWork, IListingQueueEntryWriteService listingQueueEntryWriteService)
        {
            _listingGroupReadService = listingGroupReadService;
            _listingGroupWriteService = listingGroupWriteService;
            _listingReadService = listingReadService;
            _listingWriteService = listingWriteService;
            _listingItemReadService = listingItemReadService;
            _listingItemWriteService = listingItemWriteService;
            _unitOfWork = unitOfWork;
            _listingQueueEntryWriteService = listingQueueEntryWriteService;
        }

        private async Task<ListingItem> UpdateListingItem(Guid listingId, UpdateListingItemRequest request)
        {
            if (request.Id == null)
            {
                ListingItem listingItemToAdd = new ListingItem()
                {
                    Id = Guid.NewGuid(),
                    ListingId = listingId,
                    CatalogId = request.CatalogId,
                    Details = request.Details,
                };
                await _listingItemWriteService.AddListingItemAsync(listingItemToAdd);

                return listingItemToAdd;
            }

            ListingItem listingItemToUpdate = new ListingItem()
            {
                Id = request.Id.Value,
                ListingId = listingId,
                CatalogId = request.CatalogId,
                Details = request.Details,
            };
            await _listingItemWriteService.UpdateListingItemAsync(listingItemToUpdate);

            return listingItemToUpdate;
        }

        private async Task UpdateListingItems(Guid listingId, IEnumerable<UpdateListingItemRequest> requests)
        {
            Dictionary<Guid, ListingItem> listingItemsFromUpdate = new Dictionary<Guid, ListingItem>();
            foreach (UpdateListingItemRequest request in requests)
            {
                ListingItem item = await UpdateListingItem(listingId, request);
                listingItemsFromUpdate[item.Id] = item;
            }
            List<ListingItem> existingListingItems = (await _listingItemReadService.GetListingItemsByListingIdAsync(listingId)).ToList();

            // Delete removed listingToAdd items
            foreach (ListingItem existingListingItem in existingListingItems)
            {
                if (!listingItemsFromUpdate.ContainsKey(existingListingItem.Id))
                {
                   await _listingItemWriteService.DeleteListingItemAsync(existingListingItem.Id);
                }
            }
        }
        private async Task<Listing> UpdateListing(Guid listingGroupId, UpdateListingRequest request)
        {
            if (request.Id == null)
            {
                Listing listingToAdd = new Listing()
                {
                    Id = Guid.NewGuid(),
                    ListingGroupId = listingGroupId,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    Title = request.Title,
                    Details = request.Details,
                    Price = request.Price,
                    Status = ListingStatus.Active,
                };
                await _listingWriteService.AddListingAsync(listingToAdd);

                return listingToAdd;
            }

            Listing? existingListing = await _listingReadService.GetListingByIdAsync(request.Id.Value);

            if (existingListing == null)
            {
                throw new InvalidOperationException();
            }

            Listing listingToUpdate = new Listing()
            {
                Id = existingListing.Id,
                ListingGroupId = listingGroupId,
                CreatedAt = existingListing.CreatedAt,
                ModifiedAt = DateTime.Now,
                Title = request.Title,
                Details = request.Details,
                Price = request.Price,
                Status = existingListing.Status,
            };
            await _listingWriteService.UpdateListingAsync(listingToUpdate);

            return listingToUpdate;
        }
        private async Task UpdateListings(Guid listingGroupId, IEnumerable<UpdateListingRequest> requests)
        {
            Dictionary<Guid, Listing> listingsFromUpdate = new Dictionary<Guid, Listing>();
            List<ListingAggregate> listingAggregates = new List<ListingAggregate>();
            foreach (UpdateListingRequest request in requests)
            {
                Listing listingFromUpdate = await UpdateListing(listingGroupId, request);
                listingsFromUpdate[listingFromUpdate.Id] = listingFromUpdate;
                
                await UpdateListingItems(listingFromUpdate.Id, request.ListingItems);
            }

            List<Listing> existingListings = (await _listingReadService.GetListingsByListingGroupIdAsync(listingGroupId)).ToList();

            // Delete removed listings
            foreach (Listing existingListing in existingListings)
            {
                if (!listingsFromUpdate.ContainsKey(existingListing.Id))
                {
                    await _listingWriteService.DeleteListingAsync(existingListing.Id);

                    await _listingQueueEntryWriteService.DeleteListingQueueEntriesByListingIdAsync(existingListing.Id);
                }
            }
        }


        public async Task<Guid> ExecuteAsync(UpdateListingGroupRequest request, Guid requesterUserId)
        {
            ListingGroup? listingGroupFromDb = await _listingGroupReadService.GetListingGroupByIdAsync(request.Id);
            if (listingGroupFromDb == null)
            {
                throw new KeyNotFoundException($"{request.Id} of {nameof(request.Id)} not found");
            }
            if (listingGroupFromDb.SellerUserId != requesterUserId)
            {
                throw new UnauthorizedAccessException();
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                ListingGroup listingGroupToUpdate = new ListingGroup()
                {
                    Id = request.Id,
                    CreatedAt = listingGroupFromDb.CreatedAt,
                    ModifiedAt = DateTime.Now,
                    SellerUserId = listingGroupFromDb.SellerUserId,
                    PickupOptions = request.PickupOptions,
                    Details = request.Details,
                    Location = request.Location,
                };
                await _listingGroupWriteService.UpdateListingGroupAsync(listingGroupToUpdate);

                await UpdateListings(listingGroupToUpdate.Id, request.Listings);

                await _unitOfWork.CommitTransactionAsync();

                return listingGroupToUpdate.Id;
            }
            catch
            {
                await _unitOfWork.RollBackTransactionAsync();
                throw;
            }
        }
    }
}
