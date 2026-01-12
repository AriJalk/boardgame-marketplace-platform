using TestingService.Domains.CatalogItems.Dtos;
using TestingService.Domains.CatalogItems.ServiceClients;

namespace TestingService.Domains.CatalogItems.Helpers
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
