using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;
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
    internal class ListingQueueEntryRepository : IListingQueueEntryRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListingQueueEntryRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> AddListingQueueEntryAsync(ListingQueueEntry listingQueueEntry)
        {
            string sql =
                "INSERT INTO listing_queue_entries " +
                "(id, listing_id, status, buyer_user_id, created_at, modified_at, priority) " +
                "VALUES (@Id, @ListingId, @Status, @BuyerUserId, @CreatedAt, @ModifiedAt, @Priority) " +
                "ON CONFLICT (id) DO NOTHING " +
                "RETURNING id;";

            return await _unitOfWork.ExecuteAsync(sql, listingQueueEntry);
        }

        public async Task<int> DeleteListingQueueEntiresByListingIdAsync(Guid listingId)
        {
            string sql =
                "DELETE FROM listing_queue_entries " +
                "WHERE listing_id=@ListingId;";

            return await _unitOfWork.ExecuteAsync(sql, DapperHelper.Param("ListingId", listingId));
        }

        public async Task<int> DeleteListingQueueEntryAsync(Guid listingQueueEntryId)
        {
            string sql =
                "DELETE FROM listing_queue_entries " +
                "WHERE id=@Id;";

            return await _unitOfWork.ExecuteAsync(sql, DapperHelper.Param("Id", listingQueueEntryId));
        }

        public async Task<IEnumerable<ListingQueueEntry>> GetFilteredListingQueueEntriesAsync(ListingQueueEntryFilter filter)
        {
            if (filter.UserId == null && filter.ListingIds == null && filter.ListingQueueEntryIds == null && filter.ListingStatus == null) {
                return Enumerable.Empty<ListingQueueEntry>();
            }
            StringBuilder sql = new StringBuilder();
            DynamicParameters parameters = new DynamicParameters();

            sql.Append("SELECT * FROM listing_queue_entries WHERE ");

            StringBuilder filterQuery = new StringBuilder();
            if (filter.UserId != null)
            {
                filterQuery.Append("buyer_user_id=@UserId ");
                parameters.Add("UserId", filter.UserId);
            }
                if (filter.ListingIds != null && filter.ListingIds.Any())
                {
                    if (filterQuery.Length > 0)
                    {
                        filterQuery.Append("AND ");
                    }
                    filterQuery.Append("listing_id = ANY(@ListingIds) ");
                    parameters.Add("ListingIds", filter.ListingIds.ToList());
                }
            if (filter.ListingQueueEntryIds != null && filter.ListingQueueEntryIds.Any())
            {
                if (filterQuery.Length > 0)
                {
                    filterQuery.Append("AND ");
                }
                filterQuery.Append("id = ANY (@ListingQueueEntryIds) ");
                parameters.Add("ListingQueueEntryIds", filter.ListingQueueEntryIds.ToList());
            }
            if (filter.ListingStatus != null)
            {
                if (filterQuery.Length > 0)
                {
                    filterQuery.Append("AND ");
                }
                filterQuery.Append("EXISTS (SELECT 1 FROM listings l WHERE l.status=@ListingStatus) ");
                parameters.Add("ListingStatus", filter.ListingStatus);
            }

            sql.Append(filterQuery.ToString());

            sql.Append(';');

            return await _unitOfWork.QueryAsync<ListingQueueEntry>(sql.ToString(), parameters);
        }

        public async Task<ListingQueueEntry?> GetListingQueueEntryByIdAsync(Guid listingQueueEntryId)
        {
            string sql = "SELECT * FROM listing_queue_entries WHERE id=@Id";

            return await _unitOfWork.QueryFirstOrDefaultAsync<ListingQueueEntry>(sql, DapperHelper.Param("Id", listingQueueEntryId));
        }

        public Task<int> UpdateListingQueueEntriesAsync(IEnumerable<ListingQueueEntry> listingQueueEntries)
        {
            string sql =
                "UPDATE listing_queue_entries " +
                "SET status=@Status, modified_at=@ModifiedAt, priority=@Priority " +
                "WHERE id=@Id;";

            return _unitOfWork.ExecuteAsync(sql, listingQueueEntries.ToList());
        }

        public Task<int> UpdateListingQueueEntryAsync(ListingQueueEntry listingQueueEntry)
        {
            string sql =
                "UPDATE listing_queue_entries " +
                "SET status=@Status, modified_at=@ModifiedAt, priority=@Priority " +
                "WHERE id=@Id;";

            return _unitOfWork.ExecuteAsync(sql, listingQueueEntry);
        }
    }
}
