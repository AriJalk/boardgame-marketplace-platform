using BoardGameMarket.CatalogService.Core.Application.Contracts;
using BoardGameMarket.CatalogService.Core.Domain.Entities;
using BoardGameMarket.CatalogService.Core.Domain.Enums;
using BoardGameMarket.CatalogService.Core.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.CatalogService.Core.Application.Mappers
{
	public static class CatalogItemMappers
	{
		const string BGG_URL = "https://www.boardgamegeek.com/boardgame/{0}";
		public static CatalogItemInfo ToCatalogItemInfo(this CatalogItem catalogItem)
		{
			CatalogItemType type = catalogItem.Type;
			string? bggUrl = type == CatalogItemType.Bgg ? string.Format(BGG_URL, catalogItem.BggId) : null;
			return new CatalogItemInfo(catalogItem.Id, type, catalogItem.Name, bggUrl, catalogItem.ImageUrl);
		}
	}
}
