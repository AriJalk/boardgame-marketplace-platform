using BoardGameMarket.ListingsService.Core.Application.Access.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingItems;
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

        public UpdateListingGroupUseCase(IListingGroupReadService listingGroupReadService, IListingGroupWriteService listingGroupWriteService, IListingReadService listingReadService, IListingWriteService listingWriteService, IListingItemReadService listingItemReadService, IListingItemWriteService listingItemWriteService, IUnitOfWork unitOfWork)
        {
            _listingGroupReadService = listingGroupReadService;
            _listingGroupWriteService = listingGroupWriteService;
            _listingReadService = listingReadService;
            _listingWriteService = listingWriteService;
            _listingItemReadService = listingItemReadService;
            _listingItemWriteService = listingItemWriteService;
            _unitOfWork = unitOfWork;
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
                ListingItem? listingItemFromCreate = await _listingItemWriteService.AddListingItemAsync(listingItemToAdd);

                if (listingItemFromCreate == null)
                {
                    throw new InvalidOperationException("Can't create listingItemFromUpdate");
                }

                return listingItemFromCreate;
            }

            ListingItem listingItemToUpdate = new ListingItem()
            {
                Id = request.Id.Value,
                ListingId = listingId,
                CatalogId = request.CatalogId,
                Details = request.Details,
            };
            ListingItem? listingItemFromUpdate = await _listingItemWriteService.UpdateListingItemAsync(listingItemToUpdate);
            if (listingItemFromUpdate == null)
            {
                throw new InvalidOperationException();
            }
            return listingItemToUpdate;
        }

        private async Task<IEnumerable<ListingItem>> UpdateListingItems(Guid listingId, IEnumerable<UpdateListingItemRequest> requests)
        {
            Dictionary<Guid, ListingItem> listingItemsFromUpdate = new Dictionary<Guid, ListingItem>();
            foreach (UpdateListingItemRequest request in requests)
            {
                ListingItem item = await UpdateListingItem(listingId, request);
                listingItemsFromUpdate[item.Id] = item;
            }
            IEnumerable<ListingItem> existingListingItems = await _listingItemReadService.GetListingItemsByListingIdAsync(listingId);
            foreach (ListingItem existingListingItem in existingListingItems)
            {
                if (!listingItemsFromUpdate.ContainsKey(existingListingItem.Id))
                {
                    if (!await _listingItemWriteService.DeleteListingItemAsync(existingListingItem.Id))
                    {
                        throw new InvalidOperationException();
                    }
                }
            }

            return listingItemsFromUpdate.Values;
        }
        private async Task<Listing> UpdateListing(Guid listingGroupId, UpdateListingRequest request)
        {
            if (request.Id == null)
            {
                Listing listing = new Listing()
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
                Listing? listingFromAdd = await _listingWriteService.AddListingAsync(listing);
                if (listingFromAdd == null)
                {
                    throw new InvalidOperationException();
                }
                return listingFromAdd;
            }

            Listing? listingFromDb = await _listingReadService.GetListingByIdAsync(request.Id.Value);

            if (listingFromDb == null)
            {
                throw new InvalidOperationException();
            }

            Listing listingToUpdate = new Listing()
            {
                Id = listingFromDb.Id,
                ListingGroupId = listingGroupId,
                CreatedAt = listingFromDb.CreatedAt,
                ModifiedAt = DateTime.Now,
                Title = request.Title,
                Details = request.Details,
                Price = request.Price,
                Status = listingFromDb.Status,
            };
            Listing? listingFromUpdate = await _listingWriteService.UpdateListingAsync(listingToUpdate);
            if (listingFromUpdate == null)
            {
                throw new InvalidOperationException();
            }
            return listingFromUpdate;
        }
        private async Task<IEnumerable<ListingAggregate>> UpdateListings(Guid listingGroupId, IEnumerable<UpdateListingRequest> requests)
        {
            List<Listing> existingListings = (await _listingReadService.GetListingsByListingGroupIdAsync(listingGroupId)).ToList();
            Dictionary<Guid, Listing> listingsFromUpdate = new Dictionary<Guid, Listing>();
            List<ListingAggregate> listingAggregates = new List<ListingAggregate>();
            foreach (UpdateListingRequest request in requests)
            {
                Listing listingFromUpdate = await UpdateListing(listingGroupId, request);
                listingsFromUpdate[listingFromUpdate.Id] = listingFromUpdate;
                IEnumerable<ListingItem> listingItems = await UpdateListingItems(listingFromUpdate.Id, request.ListingItems);
                listingAggregates.Add(new ListingAggregate(listingFromUpdate, listingItems.ToList()));
            }

            foreach (Listing existingListing in existingListings)
            {
                if (!listingsFromUpdate.ContainsKey(existingListing.Id))
                {
                    if (!await _listingWriteService.DeleteListingAsync(existingListing.Id))
                    {
                        throw new InvalidOperationException();
                    }
                }
            }


            return listingAggregates;
        }


        public async Task<ListingGroupDetail> ExecuteAsync(UpdateListingGroupRequest request, Guid requesterUserId)
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
                ListingGroup? listingGroupFromUpdate = await _listingGroupWriteService.UpdateListingGroupAsync(listingGroupToUpdate);
                if (listingGroupFromUpdate == null)
                {
                    throw new InvalidOperationException();
                }
                IEnumerable<ListingAggregate> listingsFromUpdate = await UpdateListings(listingGroupFromUpdate.Id, request.Listings);

                await _unitOfWork.CommitTransactionAsync();

                ListingGroupAggregate listingGroupAggregate = new ListingGroupAggregate(listingGroupFromUpdate, listingsFromUpdate.ToList());
                return listingGroupAggregate.ToListingGroupDetail();
            }
            catch
            {
                await _unitOfWork.RollBackTransactionAsync();
                throw;
            }
        }
    }
}
