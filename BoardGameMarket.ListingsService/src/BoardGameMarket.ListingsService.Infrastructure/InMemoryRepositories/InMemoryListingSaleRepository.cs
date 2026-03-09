using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Infrastructure.InMemoryRepositories
{
	internal class InMemoryListingSaleRepository : IListingSaleRepository
	{
		private readonly List<ListingSale> _listingSales = new List<ListingSale>();

		public Task<int> AddListingSaleAsync(ListingSale listingSale)
		{
			_listingSales.Add(listingSale);
			return Task.FromResult(1);
		}

		public Task<int> DeleteListingSaleAsync(Guid listingSaleId)
		{
			int index = _listingSales.FindIndex(ls => ls.Id == listingSaleId);
			if (index == -1)
			{
				return Task.FromResult(0);
			}
			_listingSales.RemoveAt(index);
			return Task.FromResult(1);
		}

		public Task<ListingSale?> GetListingSaleByIdAsync(Guid listingSaleId)
		{
			return Task.FromResult(_listingSales.FirstOrDefault(l => l.Id == listingSaleId));
		}

		public Task<ListingSale?> GetListingSaleByListingIdAsync(Guid listingId)
		{
			return Task.FromResult(_listingSales.FirstOrDefault(l => l.ListingId == listingId));
		}

		public Task<ListingSale?> GetListingSaleByListingQueueEntryIdAsync(Guid listingQueueEntryId)
		{
			return Task.FromResult(_listingSales.FirstOrDefault(l => l.ListingQueueEntryId == listingQueueEntryId));
		}

		public Task<IEnumerable<ListingSale>> GetListingSalesByBuyerUserIdAsync(Guid buyerUserId)
		{
			return Task.FromResult(_listingSales.Where(l => l.BuyerUserId == buyerUserId));
		}

		public Task<IEnumerable<ListingSale>> GetListingSalesByDateRange(DateTime startDate, DateTime endDate)
		{
			return Task.FromResult(_listingSales.Where(l => l.CompletedAt >= startDate && l.CompletedAt <= endDate));
		}

		public Task<IEnumerable<ListingSale>> GetListingSalesBySellerUserIdAsync(Guid sellerUserId)
		{
			return Task.FromResult(_listingSales.Where(l => l.SellerUserId == sellerUserId));
		}

        public Task<IEnumerable<ListingSale>> GetListingSalesByUserIdAsync(Guid userId)
        {
            return Task.FromResult(_listingSales.Where(l => l.SellerUserId == userId || l.BuyerUserId == userId));
        }
    }
}
