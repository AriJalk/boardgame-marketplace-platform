using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using BoardGameMarket.ListingsService.Infrastructure.Helpers;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BoardGameMarket.ListingsService.Infrastructure.Repositories
{
    internal class ListingReservationRepository : IListingReservationRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListingReservationRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> AddListingReservationAsync(ListingReservation listingReservation)
        {
            string sql =
                "INSERT INTO listing_reservations " +
                "(id, created_at, modified_at, status, listing_queue_entry_id, listing_id, seller_user_id, buyer_user_id) " +
                "VALUES (@Id, @CreatedAt, @ModifiedAt, @Status, @ListingQueueEntryId, @ListingId, @SellerUserId, @BuyerUserId) " +
                "ON CONFLICT (id) DO NOTHING " +
                "RETURNING id;";

            return await _unitOfWork.ExecuteAsync(sql, listingReservation);
        }

        public async Task<int> DeleteListingReservationAsync(Guid listingReservationId)
        {
            string sql = "DELETE FROM listing_reservations WHERE id=@Id;";

            return await _unitOfWork.ExecuteAsync(sql, DapperHelper.Param("Id", listingReservationId));
        }

        public async Task<ListingReservation?> GetListingReservationByIdAsync(Guid listingReservationId)
        {
            string sql = "SELECT * FROM listing_reservations WHERE id=@Id;";

            return await _unitOfWork.QueryFirstOrDefaultAsync<ListingReservation>(sql, DapperHelper.Param("Id", listingReservationId));
        }

        public async Task<ListingReservation?> GetListingReservationByListingIdAsync(Guid listingId)
        {
            string sql = "SELECT * FROM listing_reservations WHERE listing_id=@ListingId;";

            return await _unitOfWork.QueryFirstOrDefaultAsync<ListingReservation>(sql, DapperHelper.Param("ListingId", listingId));
        }

        public async Task<IEnumerable<ListingReservation>> GetListingsReservationsByBuyerUserIdAsync(Guid buyerUserId)
        {
            string sql = "SELECT * FROM listing_reservations WHERE buyer_user_id=@BuyerUserId;";

            return await _unitOfWork.QueryAsync<ListingReservation>(sql, DapperHelper.Param("BuyerUserId", buyerUserId));
        }

        public async Task<IEnumerable<ListingReservation>> GetListingsReservationsBySellerUserIdAsync(Guid sellerUserId)
        {
            string sql = "SELECT * FROM listing_reservations WHERE seller_user_id=@SellerUserId;";

            return await _unitOfWork.QueryAsync<ListingReservation>(sql, DapperHelper.Param("SellerUserId", sellerUserId));
        }

        public async Task<int> UpdateListingReservationAsync(ListingReservation listingReservation)
        {
            string sql =
                "UPDATE listing_reservations " +
                "SET modified_at=@ModifiedAt, status=@Status " +
                "WHERE id=@Id;";

            return await _unitOfWork.ExecuteAsync(sql, listingReservation);
        }
    }
}
