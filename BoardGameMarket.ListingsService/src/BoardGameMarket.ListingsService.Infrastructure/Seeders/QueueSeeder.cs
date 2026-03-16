using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace BoardGameMarket.ListingsService.Infrastructure.Seeders
{


    public class QueueSeeder
    {
        private const int PRIORITY_GAP = 1000;
        private readonly IListingGroupRepository _listingGroupRepository;
        private readonly IListingRepository _listingRepository;
        private readonly IListingQueueEntryRepository _listingQueueEntryRepository;

        private readonly JsonSerializerOptions _jsonOptions;

        private IEnumerable<Guid> _userIds = Enumerable.Empty<Guid>();

        private struct UserInfo
        {
            public Guid Id { get; set; }
        }

        public QueueSeeder(IListingGroupRepository listingGroupRepository, IListingRepository listingRepository, IListingQueueEntryRepository listingQueueEntryRepository, JsonSerializerOptions jsonOptions)
        {
            _listingGroupRepository = listingGroupRepository;
            _listingRepository = listingRepository;
            _listingQueueEntryRepository = listingQueueEntryRepository;
            _jsonOptions = jsonOptions;
        }

        private Guid CreateDeterministicGuid(Guid listingId, int index)
        {
            using var md5 = MD5.Create();
            var inputBytes = Encoding.UTF8.GetBytes($"{listingId}-{index}");
            var hash = md5.ComputeHash(inputBytes);
            return new Guid(hash);
        }

        public async Task SeedAsync(string seedPath)
        {
            await LoadUserIds(seedPath);

            IEnumerable<Listing> listings = await _listingRepository.GetListings();
            foreach (Listing listing in listings)
            {
                ListingGroup? listingGroup = await _listingGroupRepository.GetListingGroupByIdAsync(listing.ListingGroupId);
                if (listingGroup == null)
                {
                    throw new KeyNotFoundException();
                }
                await CreateQueueForListing(listing, listingGroup.SellerUserId);
            }
        }

        private async Task LoadUserIds(string seedPath)
        {
            string json = File.ReadAllText(Path.Combine(seedPath, "_Shared", "USER_IDS.json"));
            IEnumerable<UserInfo>? userIds = JsonSerializer.Deserialize<IEnumerable<UserInfo>>(json, _jsonOptions);

            if (userIds == null)
            {
                return;
            }

            _userIds = userIds.Select(u => u.Id);
        }

        private async Task CreateQueueForListing(Listing listing, Guid SellerUserId)
        {
            Guid[] validBuyers = _userIds.Where(id => id != SellerUserId).ToArray();

            for (int i = 0; i < validBuyers.Count(); i++)
            {
                ListingQueueEntry queueEntry = new ListingQueueEntry()
                {
                    Id = CreateDeterministicGuid(listing.Id, i),
                    ListingId = listing.Id,
                    BuyerUserId = validBuyers[i],
                    CreatedAt = DateTime.UtcNow.AddDays(-i),
                    ModifiedAt = DateTime.UtcNow,
                    Status = QueueStatus.Queued
                };
                if (i % 2 == 0)
                {
                    queueEntry.Priority = i * PRIORITY_GAP;
                }
                await _listingQueueEntryRepository.AddListingQueueEntryAsync(queueEntry);
            }
        }
    }
}
