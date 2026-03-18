using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.Pagination;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using BoardGameMarket.ListingsService.Infrastructure.Helpers;
using Dapper;
using Npgsql;
using System.Data;
using System.Text;

namespace BoardGameMarket.ListingsService.Infrastructure.Repositories
{
    internal class ListingGroupRepository : IListingGroupRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListingGroupRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private List<string> GetFilterQuery(ListingGroupFilter filter, DynamicParameters parameters)
        {
            List<string> conditions = new List<string>();
            if (filter.SellerUserId != null)
            {
                conditions.Add("seller_user_id=@SellerUserId");
                parameters.Add("SellerUserId", filter.SellerUserId);
            }
            if (filter.CatalogId != null)
            {
                conditions.Add(
                    "EXISTS (" +
                        "SELECT 1 FROM listings l " +
                        "WHERE l.listing_group_id = lg.id " +
                        "AND EXISTS (" +
                            "SELECT 1 FROM listing_items li " +
                            "WHERE li.listing_id = l.id " +
                            "AND li.catalog_id = @CatalogId" +
                        ")" +
                    ") ");
                parameters.Add("CatalogId", filter.CatalogId);
            }
            if (filter.Location != null)
            {
                conditions.Add("location=@Location ");
                parameters.Add("Location", filter.Location);
            }
            if (filter.ListingGroupIds != null && filter.ListingGroupIds.Any())
            {
                conditions.Add("id = ANY(@ListingGroupIds) ");
                parameters.Add("ListingGroupIds", filter.ListingGroupIds.ToList());
            }
            return conditions;
        }

        public async Task<int> AddListingGroupAsync(ListingGroup listingGroup)
        {
            string sql =
                "INSERT INTO listing_groups (id, seller_user_id,created_at, modified_at, details, pickup_options, location) " +
                "VALUES (@Id, @SellerUserId, @CreatedAt, @ModifiedAt, @Details, @PickupOptions, @Location) " +
                "ON CONFLICT (id) DO NOTHING " +
                "RETURNING id;";

            return await _unitOfWork.ExecuteAsync(sql, listingGroup);
        }

        public async Task<int> DeleteListingGroupAsync(Guid listingGroupId)
        {
            string sql = "DELETE FROM listing_groups WHERE id = @Id;";

            return await _unitOfWork.ExecuteAsync(sql, DapperHelper.Param("Id", listingGroupId));
        }

        public async Task<IEnumerable<ListingGroup>> GetFilteredListingGroupsAsync(ListingGroupFilter filter)
        {
            DynamicParameters parameters = new DynamicParameters();

            List<string> conditions = GetFilterQuery(filter, parameters);

            if (conditions.Count == 0)
            {
                return Enumerable.Empty<ListingGroup>();
            }

            StringBuilder sql = new StringBuilder("SELECT * FROM listing_groups lg WHERE ");
            sql.Append(string.Join(" AND ", conditions)).Append(";");
            return await _unitOfWork.QueryAsync<ListingGroup>(sql.ToString(), parameters);
        }

        public async Task<ListingGroup?> GetListingGroupByIdAsync(Guid listingGroupId)
        {
            string sql = "SELECT * FROM listing_groups WHERE id=@Id;";

            return await _unitOfWork.QueryFirstOrDefaultAsync<ListingGroup>(sql, DapperHelper.Param("Id", listingGroupId));
        }

        public async Task<ListingGroup?> GetListingGroupByListingIdAsync(Guid listingId)
        {
            string sql =
                "SELECT * FROM listing_groups lg WHERE " +
                "EXISTS(SELECT 1 FROM listings l WHERE l.id=@ListingId);";

            return await _unitOfWork.QueryFirstOrDefaultAsync<ListingGroup>(sql, DapperHelper.Param("ListingId", listingId));
        }

        public async Task<IEnumerable<ListingGroup>> GetListingGroupsByIds(IEnumerable<Guid> listingGroupIds)
        {
            string sql = "SELECT * FROM listing_groups WHERE id = ANY(@Ids);";

            return await _unitOfWork.QueryAsync<ListingGroup>(sql, DapperHelper.Param("Ids", listingGroupIds.ToList()));
        }

        public async Task<IEnumerable<ListingGroup>> GetListingGroupsBySellerUserId(Guid sellerUserId)
        {
            string sql = "SELECT * FROM listing_groups WHERE seller_user_id=@SellerUserId;";

            return await _unitOfWork.QueryAsync<ListingGroup>(sql, DapperHelper.Param("SellerUserId", sellerUserId));
        }

        public async Task<PagedResult<ListingGroup>> GetListingGroupsPageAsync(Cursor? cursor, int pageSize, ListingGroupFilter? filter)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder sql = new StringBuilder();

            parameters.Add("PageSize", pageSize + 1);

            sql.Append("SELECT * FROM listing_groups lg ");
            List<string> conditions = new List<string>();

            if (cursor != null)
            {
                conditions.Add("(lg.created_at, lg.id)<(@LastCreatedAt,@LastId)");
                parameters.Add("LastCreatedAt", cursor.LastCreatedAt);
                parameters.Add("LastId", cursor.LastId);
            }
            if (filter != null)
            {
                 conditions.AddRange(GetFilterQuery(filter, parameters));
            }

            if (conditions.Any())
            {
                sql.Append("WHERE ").Append(string.Join(" AND ", conditions)).Append(' ');
            }

            sql.Append("ORDER BY created_at DESC, id DESC ");
            sql.Append("LIMIT @PageSize;");

            List<ListingGroup> listingGroups = (await _unitOfWork.QueryAsync<ListingGroup>(sql.ToString(), parameters)).ToList();

            string? nextCursor = null;

            bool hasNext = listingGroups.Count > pageSize;

            if (hasNext)
            {
                listingGroups.RemoveAt(listingGroups.Count - 1);
                ListingGroup last = listingGroups.Last();
                nextCursor = $"{last.CreatedAt:o}|{last.Id}";
                nextCursor = Convert.ToBase64String(Encoding.UTF8.GetBytes(nextCursor));
            }

            return new PagedResult<ListingGroup>()
            {
                Items = listingGroups.ToArray(),
                NextCursor = nextCursor,
            };
        }

        public async Task<int> UpdateListingGroupAsync(ListingGroup listingGroup)
        {
            string sql =
                "UPDATE listing_groups " +
                "SET modified_at=@ModifiedAt, details=@Details, pickup_options=@PickupOptions," +
                "location=@Location " +
                "WHERE id=@Id;";

            return await _unitOfWork.ExecuteAsync(sql, listingGroup);
        }
    }
}
