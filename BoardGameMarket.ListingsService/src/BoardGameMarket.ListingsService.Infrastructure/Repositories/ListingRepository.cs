using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using BoardGameMarket.ListingsService.Infrastructure.Helpers;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Infrastructure.Repositories
{
    internal class ListingRepository : IListingRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListingRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> AddListingAsync(Listing listing)
        {
            string sql = "" +
                "INSERT INTO listings (id, title, listing_group_id, status, created_at, modified_at, price, details, seller_user_id) " +
                "VALUES (@Id, @Title, @ListingGroupId, @Status, @CreatedAt, @ModifiedAt, @Price, @Details, @SellerUserId) " +
                "ON CONFLICT (id) DO NOTHING " +
                "RETURNING id;";

            return await _unitOfWork.ExecuteAsync(sql, listing);
        }

        public async Task<int> AddListingsAsync(IEnumerable<Listing> listings)
        {
            string sql =
                "INSERT INTO listings (id, title,listing_group_id, status, created_at, modified_at, price, details, seller_user_id) " +
                "VALUES (@Id, @Title, @ListingGroupId, @Status, @CreatedAt, @ModifiedAt, @Price, @Details, @SellerUserId) " +
                "ON CONFLICT (id) DO NOTHING " +
                "RETURNING id;";

            return await _unitOfWork.ExecuteAsync(sql, listings.ToList());
        }

        public async Task<int> DeleteListingAsync(Guid listingId)
        {
            string sql = "DELETE FROM listings WHERE id=@Id;";

            return await _unitOfWork.ExecuteAsync(sql, DapperHelper.Param("Id", listingId));
        }

        public async Task<int> DeleteListingsByListingGroupIdAsync(Guid listingGroupId)
        {
            string sql = "DELTE FROM listings WHERE listing_group_id=@ListingGroupId;";

            return await _unitOfWork.ExecuteAsync(sql, DapperHelper.Param("ListingGroupId", listingGroupId));
        }

        public async Task<Listing?> GetListingByIdAsync(Guid listingId)
        {
            string sql = "SELECT * FROM listings WHERE id=@Id;";

            return await _unitOfWork.QueryFirstOrDefaultAsync<Listing>(sql, DapperHelper.Param("Id", listingId));
        }

        public async Task<IEnumerable<Listing>> GetListings()
        {
            string sql = "SELECT * FROM listings";

            return await _unitOfWork.QueryAsync<Listing>(sql, null);
        }

        public async Task<IEnumerable<Listing>> GetListingsByListingGroupIdAsync(Guid listingGroupId)
        {
            string sql = "SELECT * FROM listings WHERE listing_group_id=@ListingGroupId;";

            return await _unitOfWork.QueryAsync<Listing>(sql, DapperHelper.Param("ListingGroupId", listingGroupId));
        }

        public async Task<IEnumerable<Listing>> GetListingsByListingIdsAsync(IEnumerable<Guid> listingIds)
        {
            string sql = "SELECT * FROM listings WHERE id = ANY(@Ids);";

            return await _unitOfWork.QueryAsync<Listing>(sql, DapperHelper.Param("Ids", listingIds.ToList()));
        }

        public async Task<int> UpdateListingAsync(Listing listing)
        {
            string sql = "" +
                "UPDATE listings " +
                "SET title=@Title, status=@Status, modified_at=@ModifiedAt, price=@Price, details=@Details " +
                "WHERE id=@Id";

            return await _unitOfWork.ExecuteAsync(sql, listing);
        }

        public async Task<int> UpdateListingsAsync(IEnumerable<Listing> listings)
        {
            string sql = "" +
                "UPDATE listings " +
                "SET title=@Title, status=@Status, modified_at=@ModifiedAt, price=@Price, details=@Details " +
                "WHERE id=@Id";

            return await _unitOfWork.ExecuteAsync(sql, listings.ToList());
        }
    }
}
