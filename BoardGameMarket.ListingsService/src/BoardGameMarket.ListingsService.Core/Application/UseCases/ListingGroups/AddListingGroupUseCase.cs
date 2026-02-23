using BoardGameMarket.ListingsService.Core.Application.Access.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.Contracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Domain.Aggragates;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
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

        private Listing CreateListing(CreateListingRequest request, Guid listingGroupId)
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
            };
        }

        private ListingItem CreateListingItem(CreateListingItemRequest request, Guid listingId)
        {
            return new ListingItem()
            {
                Id = Guid.NewGuid(),
                ListingId = listingId,
                CatalogId = request.CatalogId,
                Details = request.Details,
            };
        }

        public async Task<ListingGroupDetail> ExecuteAsync(CreateListingGroupRequest listingGroupRequest)
        {
            // Step 1: Build entities
            ListingGroup listingGroupToAdd = new ListingGroup()
            {
                Id = Guid.NewGuid(),
                SellerUserId = listingGroupRequest.SellerUserId,
                PickupOptions = listingGroupRequest.PickupOptions,
                Location = listingGroupRequest.Location,
                Details = listingGroupRequest.Details,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
            };
            List<Listing> listingsToAdd = new List<Listing>();
            Dictionary<Guid, List<ListingItem>> listingItemsDictionary = new Dictionary<Guid, List<ListingItem>>();
            foreach (CreateListingRequest listingRequest in listingGroupRequest.Listings)
            {
                Listing listingToAdd = CreateListing(listingRequest, listingGroupToAdd.Id);
                listingsToAdd.Add(listingToAdd);
                listingItemsDictionary[listingToAdd.Id] = new List<ListingItem>();
                foreach (CreateListingItemRequest listingItemRequest in listingRequest.ListingItems)
                {
                    listingItemsDictionary[listingToAdd.Id].Add(CreateListingItem(listingItemRequest, listingToAdd.Id));
                }
            }
            try
            {
                // Step 2: Persist entities
                await _unitOfWork.BeginTransactionAsync();

                ListingGroup? listingGroupFromAdd = await _listingGroupWriteService.AddListingGroup(listingGroupToAdd);
                if (listingGroupFromAdd == null)
                {
                    throw new InvalidOperationException();
                }

                IEnumerable<Listing> listingsFromAdd = await _listingWriteService.AddListingsAsync(listingsToAdd);
                IEnumerable<ListingItem> listingItemsFromAdd = await _listingItemWriteService.AddListingItemsAsync(listingItemsDictionary.Values.SelectMany(l => l));

                await _unitOfWork.CommitTransactionAsync();

                // Step 3: Build aggregates for return
                IEnumerable<ListingAggregate> listingAggregates = listingsFromAdd.Select(l => new ListingAggregate(l, listingItemsDictionary[l.Id]));
                ListingGroupAggregate listingGroupAggregate = new ListingGroupAggregate(listingGroupFromAdd, listingAggregates.ToList());

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
