using BoardGameMarket.MarketGateway.API.Application.CatalogItems.Dtos;
using BoardGameMarket.MarketGateway.API.Application.CatalogItems.ServiceClients;

namespace BoardGameMarket.MarketGateway.API.Application.CatalogItems.Mappers
{
	public static class CatalogItemMappers
	{
		public static CatalogItemDto ToCatalogItemDto(this CatalogItemInfo catalogItemInfo)
		{
			Console.WriteLine(catalogItemInfo.Type.ToString());
			return new CatalogItemDto()
			{
				Id = catalogItemInfo.Id,
				Type = catalogItemInfo.Type,
				Name = catalogItemInfo.Name,
				BggUrl = catalogItemInfo.BggUrl,
				ImageUrl = catalogItemInfo.ImageUrl,
			};
		}
	}
}
