using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingSales
{
	public interface IListingSaleWriteService
	{
		Task AddListingSaleAsync(ListingSale listingSaleToAdd);

		Task DeleteListingSaleAsync(Guid listingSaleId);
	}
}
