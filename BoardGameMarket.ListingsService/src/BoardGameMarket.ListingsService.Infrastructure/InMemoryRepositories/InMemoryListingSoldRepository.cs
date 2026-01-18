using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Infrastructure.InMemoryRepositories
{
	internal class InMemoryListingSoldRepository : IListingSoldRepository
	{
		private readonly List<ListingSold> _listingSolds = new List<ListingSold>();

		public Task<ListingSold?> AddListingSoldAsync(ListingSold listingSold)
		{
			_listingSolds.Add(listingSold);
			return Task.FromResult(listingSold);
		}

		public Task<bool> DeleteListingSoldAsync(Guid listingSoldId)
		{
			_listingSolds.Remove(_listingSolds.FirstOrDefault(l => l.Id == listingSoldId));
			return Task.FromResult(true);
		}

		public Task<ListingSold?> GetListingSoldByIdAsync(Guid listingSoldId)
		{
			return Task.FromResult(_listingSolds.FirstOrDefault(l => l.Id == listingSoldId));
		}

		public Task<ListingSold?> GetListingSoldByListingIdAsync(Guid listingId)
		{
			return Task.FromResult(_listingSolds.FirstOrDefault(l => l.ListingId == listingId));
		}

		public Task<ListingSold?> GetListingSoldByListingQueueEntryIdAsync(Guid listingQueueEntryId)
		{
			return Task.FromResult(_listingSolds.FirstOrDefault(l => l.ListingQueueEntryId == listingQueueEntryId));
		}

		public Task<IEnumerable<ListingSold>> GetListingsSoldByBuyerUserIdAsync(Guid buyerUserId)
		{
			return Task.FromResult(_listingSolds.Where(l => l.BuyerId == buyerUserId));
		}

		public Task<IEnumerable<ListingSold>> GetListingsSoldByDateRange(DateTime startDate, DateTime endDate)
		{
			return Task.FromResult(_listingSolds.Where(l => l.CompletedAt >= startDate && l.CompletedAt <= endDate));
		}

		public Task<IEnumerable<ListingSold>> GetListingsSoldBySellerUserIdAsync(Guid sellerUserId)
		{
			return Task.FromResult(_listingSolds.Where(l => l.SellerId == sellerUserId));
		}
	}
}
