using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingsSold
{
	public interface IListingSoldWriteService
	{
		Task<ListingSold?> AddListingSoldAsync(ListingSold listingSoldToAdd);

		Task<bool> DeleteListingSoldAsync(Guid listingSoldId);
	}
}
