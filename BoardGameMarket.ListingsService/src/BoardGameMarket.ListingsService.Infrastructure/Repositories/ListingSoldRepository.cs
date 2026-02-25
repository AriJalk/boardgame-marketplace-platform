using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Infrastructure.Repositories
{
    internal class ListingSoldRepository : IListingSoldRepository
    {
        public Task<int> AddListingSoldAsync(ListingSold listingSold)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteListingSoldAsync(Guid listingSoldId)
        {
            throw new NotImplementedException();
        }

        public Task<ListingSold?> GetListingSoldByIdAsync(Guid listingSoldId)
        {
            throw new NotImplementedException();
        }

        public Task<ListingSold?> GetListingSoldByListingIdAsync(Guid listingId)
        {
            throw new NotImplementedException();
        }

        public Task<ListingSold?> GetListingSoldByListingQueueEntryIdAsync(Guid listingQueueEntryId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ListingSold>> GetListingsSoldByBuyerUserIdAsync(Guid buyerUserId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ListingSold>> GetListingsSoldByDateRange(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ListingSold>> GetListingsSoldBySellerUserIdAsync(Guid sellerUserId)
        {
            throw new NotImplementedException();
        }
    }
}
