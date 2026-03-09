using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;

namespace BoardGameMarket.ListingsService.Infrastructure.Repositories
{
    internal class ListingSaleRepository : IListingSaleRepository
    {
        public Task<int> AddListingSaleAsync(ListingSale listingSale)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteListingSaleAsync(Guid listingSaleId)
        {
            throw new NotImplementedException();
        }

        public Task<ListingSale?> GetListingSaleByIdAsync(Guid listingSaleId)
        {
            throw new NotImplementedException();
        }

        public Task<ListingSale?> GetListingSaleByListingIdAsync(Guid listingId)
        {
            throw new NotImplementedException();
        }

        public Task<ListingSale?> GetListingSaleByListingQueueEntryIdAsync(Guid listingQueueEntryId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ListingSale>> GetListingSalesByBuyerUserIdAsync(Guid buyerUserId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ListingSale>> GetListingSalesByDateRange(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ListingSale>> GetListingSalesBySellerUserIdAsync(Guid sellerUserId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ListingSale>> GetListingSalesByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
