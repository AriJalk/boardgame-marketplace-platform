using BoardGameMarket.MarketGateway.API.Application.CatalogItems.Dtos;
using BoardGameMarket.MarketGateway.API.Application.CatalogItems.ServiceClients;

namespace BoardGameMarket.MarketGateway.API.Application.CatalogItems.Mappers
{
	public static class CatalogItemMappers
	{
		public static CatalogItemDto ToCatalogItemDto(this CatalogItemInfo catalogItemInfo)
		{
			string? bggLink = null;
			if (catalogItemInfo.BggId != null)
			{
				bggLink = $"https://www.boardgamegeek.com/boardgame/{catalogItemInfo.BggId}";
			}
			Console.WriteLine(catalogItemInfo.Type.ToString());
			return new CatalogItemDto()
			{
				Id = catalogItemInfo.Id,
				Type = catalogItemInfo.Type,
				Name = catalogItemInfo.Name,
				BggLink = bggLink,
				ImageUrl = catalogItemInfo.ImageUrl,
			};
		}
	}
}
