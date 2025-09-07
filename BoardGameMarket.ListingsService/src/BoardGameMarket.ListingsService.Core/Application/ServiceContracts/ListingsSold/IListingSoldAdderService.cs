using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingsSold;
using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingsSold
{
	public interface IListingSoldAdderService
	{
		Task<ListingSold?> AddListingSoldAsync(ListingSoldAddRequest? listingSoldAddRequest);
	}
}
