using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingsSold;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingsSold
{
	internal class ListingSoldDeleterService : IListingSoldDeleterService
	{
		private readonly IListingSoldRepository _listingSoldRepository;

		public ListingSoldDeleterService(IListingSoldRepository listingSoldRepository)
		{
			_listingSoldRepository = listingSoldRepository;
		}

		public async Task<bool> DeleteListingSoldAsync(Guid listingSoldId)
		{
			return await _listingSoldRepository.DeleteListingSoldAsync(listingSoldId); ;
		}
	}
}
