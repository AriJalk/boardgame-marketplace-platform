using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingsSold
{
	public interface IListingSoldWriteService
	{
		Task AddListingSoldAsync(ListingSold listingSoldToAdd);

		Task DeleteListingSoldAsync(Guid listingSoldId);
	}
}
