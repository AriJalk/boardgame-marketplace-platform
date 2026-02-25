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

		public Task<int> AddListingSoldAsync(ListingSold listingSold)
		{
			_listingSolds.Add(listingSold);
			return Task.FromResult(1);
		}

		public Task<int> DeleteListingSoldAsync(Guid listingSoldId)
		{
			int index = _listingSolds.FindIndex(ls => ls.Id == listingSoldId);
			if (index == -1)
			{
				return Task.FromResult(0);
			}
			_listingSolds.RemoveAt(index);
			return Task.FromResult(1);
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
