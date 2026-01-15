using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.Text.Json.Serialization;
using BoardGameMarket.MarketGateway.API.Application.CatalogItems.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.CatalogItems.Enums;

namespace BoardGameMarket.MarketGateway.API.Application.CatalogItems.Services
{
	public class CatalogService
	{
		private Dictionary<Guid, CatalogItemInfo> _catalogItems { get; set; } = new Dictionary<Guid, CatalogItemInfo>();

		public CatalogService(IWebHostEnvironment hostEnvironment, JsonSerializerOptions jsonOptions)
		{
			string json = File.ReadAllText(Path.Combine(hostEnvironment.ContentRootPath, "SeedData", "CATALOG_ITEMS_MOCK_DATA.json"));

			IEnumerable<CatalogItemInfo>? catalogItems = JsonSerializer.Deserialize<IEnumerable<CatalogItemInfo>>(json, jsonOptions);

			if (catalogItems == null)
			{
				return;
			}
			foreach (CatalogItemInfo catalogItem in catalogItems)
			{
				_catalogItems[catalogItem.Id] = catalogItem;
			}
		}

		public async Task<CatalogItemInfo> GetCatalogItem(Guid catalogItemId)
		{
			return _catalogItems[catalogItemId];
		}
	}
}
