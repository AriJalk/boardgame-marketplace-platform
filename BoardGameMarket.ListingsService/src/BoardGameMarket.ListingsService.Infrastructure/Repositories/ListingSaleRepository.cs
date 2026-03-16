using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using BoardGameMarket.ListingsService.Infrastructure.Helpers;
using Dapper;
using Npgsql;
using System.Data;

namespace BoardGameMarket.ListingsService.Infrastructure.Repositories
{
    internal class ListingSaleRepository : IListingSaleRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListingSaleRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> AddListingSaleAsync(ListingSale listingSale)
        {
            string sql =
                "INSERT INTO listing_sales " +
                "(id, listing_id, completed_at, reservation_id, listing_queue_entry_id, seller_user_id, buyer_user_id) " +
                "VALUES (@Id, @ListingId, @CompletedAt, @ReservationId, @ListingQueueEntryId, @SellerUserId, @BuyerUserId) " +
                "ON CONFLICT (id) DO NOTHING " +
                "RETURNING id;";

            return await _unitOfWork.ExecuteAsync(sql, listingSale);
        }

        public async Task<int> DeleteListingSaleAsync(Guid listingSaleId)
        {
            string sql = "DELETE FROM listing_sales WHERE id=@Id;";

            return await _unitOfWork.ExecuteAsync(sql, DapperHelper.Param("Id", listingSaleId));
        }

        public async Task<ListingSale?> GetListingSaleByIdAsync(Guid listingSaleId)
        {
            string sql = "SELECT * FROM listing_sales WHERE id=@Id;";

            return await _unitOfWork.QueryFirstOrDefaultAsync<ListingSale>(sql, DapperHelper.Param("Id", listingSaleId));
        }

        public async Task<ListingSale?> GetListingSaleByListingIdAsync(Guid listingId)
        {
            string sql = "SELECT * FROM listing_sales WHERE listing_id=@ListingId;";

            return await _unitOfWork.QueryFirstOrDefaultAsync<ListingSale>(sql, DapperHelper.Param("ListingId", listingId));
        }

        public async Task<ListingSale?> GetListingSaleByListingQueueEntryIdAsync(Guid listingQueueEntryId)
        {
            string sql = 
                "SELECT * FROM listing_sales WHERE listing_queue_entry=@ListingQueueEntry;";

            return await _unitOfWork.QueryFirstOrDefaultAsync<ListingSale>(sql, DapperHelper.Param("ListingQueueEntry", listingQueueEntryId));
        }

        public async Task<IEnumerable<ListingSale>> GetListingSalesByBuyerUserIdAsync(Guid buyerUserId)
        {
            string sql = "SELECT * FROM listing_sales WHERE buyer_user_id=@BuyerUserId;";

            return await _unitOfWork.QueryAsync<ListingSale>(sql, DapperHelper.Param("BuyerUserId", buyerUserId));
        }

        public async Task<IEnumerable<ListingSale>> GetListingSalesByDateRange(DateTime startDate, DateTime endDate)
        {
            string sql = 
                "SELECT * FROM listing_sales WHERE " +
                "completed_at BETWEEN @StartDate AND @EndDate;";

            return await _unitOfWork.QueryAsync<ListingSale>(sql, DapperHelper.Params(new Dictionary<string, object>{
                { "StartDate", startDate},
                {"EndDate", endDate  }
                }));
        }

        public async Task<IEnumerable<ListingSale>> GetListingSalesBySellerUserIdAsync(Guid sellerUserId)
        {
            string sql = "SELECT * FROM listing_sales WHERE seller_user_id=@SellerUserId;";

            return await _unitOfWork.QueryAsync<ListingSale>(sql, DapperHelper.Param("SellerUserId", sellerUserId));
        }

        public async Task<IEnumerable<ListingSale>> GetListingSalesByUserIdAsync(Guid userId)
        {
            string sql = 
                "SELECT * FROM listing_sales WHERE " +
                "seller_user_id=@UserId OR " +
                "buyer_user_id=@UserId;";

            return await _unitOfWork.QueryAsync<ListingSale>(sql, DapperHelper.Param("UserId", userId));
        }
    }
}
