using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingsSold
{
	public interface IListingSoldGetterService
	{
		Task<ListingSold?> GetListingSoldByIdAsync(Guid listingSoldId);
		Task<IEnumerable<ListingSold>> GetListingsSoldByBuyerUserIdAsync(Guid buyerUserId);
		Task<IEnumerable<ListingSold>> GetListingsSoldByDateRange(DateTime startDate, DateTime endDate);
		Task<IEnumerable<ListingSold>> GetListingsSoldBySellerUserIdAsync(Guid sellerUserId);
	}
}
