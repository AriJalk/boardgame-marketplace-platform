using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using BoardGameMarket.ListingsService.Infrastructure.Helpers;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Infrastructure.Repositories
{
    internal class ListingItemRepository : IListingItemRepository
    {

        private readonly IUnitOfWork _unitOfWork;

        public ListingItemRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> AddListingItemAsync(ListingItem listingItem)
        {
            string sql =
                "INSERT INTO listing_items (id, listing_id, catalog_id, details) " +
                "VALUES (@Id, @ListingId, @CatalogId, @Details) " +
                "ON CONFLICT (id) DO NOTHING" +
                "RETURNING id;";

            return await _unitOfWork.ExecuteAsync(sql, listingItem);
        }

        public async Task<int> AddListingItemsAsync(IEnumerable<ListingItem> listingItems)
        {
            string sql =
                "INSERT INTO listing_items (id, listing_id, catalog_id, details) " +
                "VALUES (@Id, @ListingId, @CatalogId, @Details) " +
                "ON CONFLICT (id) DO NOTHING " +
                "RETURNING id;";

            return await _unitOfWork.ExecuteAsync(sql, listingItems);
        }

        public async Task<int> DeleteListingItemAsync(Guid listingItemId)
        {
            string sql = "DELETE FROM listing_items WHERE id = @Id;";

            return await _unitOfWork.ExecuteAsync(sql, DapperHelper.Param("Id", listingItemId));
        }

        public async Task<int> DeleteListingItemsByListingIdAsync(Guid listingId)
        {
            string sql = "DELETE FROM listing_items WHERE listing_id=@ListingId;";

            return await _unitOfWork.ExecuteAsync(sql, DapperHelper.Param("ListingId", listingId));
        }

        public async Task<ListingItem?> GetListingItemByIdAsync(Guid listingItemId)
        {
            string sql = "SELECT * FROM listing_items WHERE id=@Id;";

            return await _unitOfWork.QueryFirstOrDefaultAsync<ListingItem>(sql, DapperHelper.Param("Id", listingItemId));
        }

        public async Task<IEnumerable<ListingItem>> GetListingItemsByCatalogIdAsync(Guid catalogId)
        {
            string sql = "SELECT * FROM listing_items WHERE catalog_id=@CatalogId;";

            return await _unitOfWork.QueryAsync<ListingItem>(sql, DapperHelper.Param("CatalogId", catalogId));
        }

        public async Task<IEnumerable<ListingItem>> GetListingItemsByListingIdsAsync(IEnumerable<Guid> listingIds)
        {
            string sql = "SELECT * FROM listing_items WHERE listing_id in @ListingIds;";

            return await _unitOfWork.QueryAsync<ListingItem>(sql, DapperHelper.Param("ListingIds", listingIds));
        }

        public async Task<IEnumerable<ListingItem>> GetListingsItemsByListingIdAsync(Guid listingId)
        {
            string sql = "SELECT * FROM listing_items WHERE listing_id=@ListingId;";

            return await _unitOfWork.QueryAsync<ListingItem>(sql, DapperHelper.Param("ListingId", listingId));
        }

        public async Task<int> UpdateListingItemAsync(ListingItem listingItem)
        {
            string sql = 
                "UPDATE listing_items " +
                "SET details=@Details " +
                "WHERE id=@Id;";

            return await _unitOfWork.ExecuteAsync(sql, listingItem);
        }

        public async Task<int> UpdateListingItemsAsync(IEnumerable<ListingItem> listingItems)
        {
            string sql = 
                "UPDATE listing_items " +
                "SET details=@Details " +
                "WHERE id=@Id;";

            return await _unitOfWork.ExecuteAsync(sql, listingItems);
        }
    }
}
