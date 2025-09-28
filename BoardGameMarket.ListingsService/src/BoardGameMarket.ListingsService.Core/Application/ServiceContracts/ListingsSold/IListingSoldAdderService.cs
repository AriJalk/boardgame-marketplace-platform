using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingsSold
{
	public interface IListingSoldAdderService
	{
		Task<ListingSold?> AddListingSoldAsync(ListingSold listingSoldToAdd);
	}
}
