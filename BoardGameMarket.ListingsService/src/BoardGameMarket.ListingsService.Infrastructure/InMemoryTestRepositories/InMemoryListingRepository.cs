using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Infrastructure.InMemoryTestRepositories
{
	internal class InMemoryListingRepository : IListingRepository
	{

		private readonly List<Listing> _listings = new List<Listing>();

		public Task<Listing?> AddListingAsync(Listing listing)
		{
			_listings.Add(listing);
			return Task.FromResult(listing);
		}

		public Task<IEnumerable<Listing>> AddListingsAsync(IEnumerable<Listing> listings)
		{
			_listings.AddRange(listings);
			return Task.FromResult(listings);
		}


		public Task<bool> DeleteListingAsync(Guid listingId)
		{
			Listing? listing = _listings.FirstOrDefault(l => l.Id == listingId);
			if (listing == null)
			{
				return Task.FromResult(false);
			}
			_listings.Remove(listing);
			return Task.FromResult(true);
		}

		public Task DeleteListingsByListingGroupIdAsync(Guid listingGroupId)
		{
			int removed = _listings.RemoveAll(l => l.ListingGroupId == listingGroupId);
			return Task.CompletedTask;
		}

		public Task<Listing?> GetListingByIdAsync(Guid listingId)
		{
			return Task.FromResult(_listings.FirstOrDefault(l => l.Id == listingId));
		}

		public Task<IEnumerable<Listing>> GetListingsByGroupIdAsync(Guid groupId)
		{
			return Task.FromResult(_listings.Where(l => l.ListingGroupId == groupId));
		}

		public Task<IEnumerable<Listing>> GetListingsByListingIdsAsync(IEnumerable<Guid> listingIds)
		{
			return Task.FromResult(_listings.Where(l => listingIds.Contains(l.Id)));
		}

		public Task<Listing?> UpdateListingAsync(Listing listing)
		{
			Listing? originalListing = _listings.FirstOrDefault(l => l.Id == listing.Id);
			_listings.Remove(originalListing);
			_listings.Add(listing);
			return Task.FromResult(listing);
		}
	}
}
