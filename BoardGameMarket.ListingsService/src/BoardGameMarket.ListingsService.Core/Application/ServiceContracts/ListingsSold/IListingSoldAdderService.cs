using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingsSold
{
	internal interface IListingSoldAdderService
	{
		Task<ListingSold?> AddListingSoldAsync(ListingSold listingSoldToAdd);
	}
}
