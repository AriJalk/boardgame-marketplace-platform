using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Infrastructure.InMemoryRepositories
{
    internal class InMemoryListingRepository : IListingRepository
	{

		private readonly List<Listing> _listings = new List<Listing>();

		public Task<int> AddListingAsync(Listing listing)
		{
			_listings.Add(listing);
			return Task.FromResult(1);
		}

		public Task<int> AddListingsAsync(IEnumerable<Listing> listings)
		{
			_listings.AddRange(listings);
			return Task.FromResult(listings.Count());
		}


		public Task<int> DeleteListingAsync(Guid listingId)
		{
			int index = _listings.FindIndex(l => l.Id == listingId);
			if (index == -1)
			{
				return Task.FromResult(0);
			}
			_listings.RemoveAt(index);
			return Task.FromResult(1);
		}

		public Task<int> DeleteListingsByListingGroupIdAsync(Guid listingGroupId)
		{
			int rows =_listings.RemoveAll(l => l.ListingGroupId == listingGroupId);
			return Task.FromResult(rows);
		}

		public Task<Listing?> GetListingByIdAsync(Guid listingId)
		{
			return Task.FromResult(_listings.FirstOrDefault(l => l.Id == listingId));
		}

		public async Task<IEnumerable<Listing>> GetListings()
		{
			return _listings;
		}

		public Task<IEnumerable<Listing>> GetListingsByGroupIdAsync(Guid groupId)
		{
			return Task.FromResult(_listings.Where(l => l.ListingGroupId == groupId));
		}

		public Task<IEnumerable<Listing>> GetListingsByListingIdsAsync(IEnumerable<Guid> listingIds)
		{
			return Task.FromResult(_listings.Where(l => listingIds.Contains(l.Id)));
		}

		public Task<int> UpdateListingAsync(Listing listing)
		{
            int index = _listings.FindIndex(l => l.Id == listing.Id);
			if (index == -1)
			{
				return Task.FromResult(0);
			}
			_listings[index] = listing;
			return Task.FromResult(1);
        }

        public Task<int> UpdateListingsAsync(IEnumerable<Listing> listings)
        {
			int rows = 0;
			foreach (Listing listing in listings)
			{
				int index = _listings.FindIndex(l =>  l.Id == listing.Id);
				if (index == -1)
					continue;
				_listings[index++] = listing;
				rows++;
			}
			return Task.FromResult(rows);
        }
    }
}
