using BoardGameMarket.ListingsService.Core.Application.Access.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.Contracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Domain.Enums;


namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingGroups
{
    public class AddListingGroupUseCase
    {
        private readonly IListingGroupWriteService _listingGroupWriteService;
        private readonly IListingWriteService _listingWriteService;
        private readonly IListingItemWriteService _listingItemWriteService;
        private readonly IUnitOfWork _unitOfWork;

        public AddListingGroupUseCase(IListingGroupWriteService listingGroupWriteService, IListingWriteService listingWriteService, IListingItemWriteService listingItemWriteService, IUnitOfWork unitOfWork)
        {
            _listingGroupWriteService = listingGroupWriteService;
            _listingWriteService = listingWriteService;
            _listingItemWriteService = listingItemWriteService;
            _unitOfWork = unitOfWork;
        }

        private Listing CreateListing(CreateListingRequest request, Guid listingGroupId, Guid sellerUserId)
        {
            return new Listing()
            {
                Id = Guid.NewGuid(),
                ListingGroupId = listingGroupId,
                Title = request.Title,
                Price = request.Price,
                Details = request.Details,
                Status = ListingStatus.Active,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                SellerUserId = sellerUserId,
            };
        }

        private ListingItem CreateListingItem(Guid listingId, CreateListingItemRequest request)
        {
            return new ListingItem()
            {
                Id = Guid.NewGuid(),
                ListingId = listingId,
                CatalogId = request.CatalogId,
                Details = request.Details,
            };
        }


        public async Task<Guid> ExecuteAsync(CreateListingGroupRequest listingGroupRequest, Guid requesterUserId)
        {
            if (listingGroupRequest.SellerUserId != requesterUserId)
            {
                throw new UnauthorizedAccessException();
            }
            // TODO: test coverage
            if (!listingGroupRequest.Listings.Any())
            {
                throw new InvalidOperationException($"{nameof(AddListingGroupUseCase)}/{nameof(ExecuteAsync)}: ListingGroup can't have 0 Listing");
            }

            // Step 1: Build entities
            ListingGroup listingGroupToAdd = new ListingGroup()
            {
                Id = Guid.NewGuid(),
                SellerUserId = listingGroupRequest.SellerUserId,
                PickupOptions = listingGroupRequest.PickupOptions,
                Location = listingGroupRequest.Location,
                Details = listingGroupRequest.Details,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
            };
            List<Listing> listingsToAdd = new List<Listing>();
            List<ListingItem> listingItemsToAdd = new List<ListingItem>();
            foreach (CreateListingRequest listingRequest in listingGroupRequest.Listings)
            {
                // TODO: Test coverage
                if (!listingRequest.ListingItems.Any())
                {
                    throw new InvalidOperationException($"{nameof(AddListingGroupUseCase)}/{nameof(ExecuteAsync)}: Listing can't have 0 ListingItems");
                }
                Listing listingToAdd = CreateListing(listingRequest, listingGroupToAdd.Id, listingGroupRequest.SellerUserId);
                listingsToAdd.Add(listingToAdd);
                foreach (CreateListingItemRequest listingItemRequest in listingRequest.ListingItems)
                {
                    listingItemsToAdd.Add(CreateListingItem(listingToAdd.Id, listingItemRequest));
                }
            }

            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                await _listingGroupWriteService.AddListingGroup(listingGroupToAdd);
                await _listingWriteService.AddListingsAsync(listingsToAdd);
                await _listingItemWriteService.AddListingItemsAsync(listingItemsToAdd);

            });

            return listingGroupToAdd.Id;
        }
    }
}
