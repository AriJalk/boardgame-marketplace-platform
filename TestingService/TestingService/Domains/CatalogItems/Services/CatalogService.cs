using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.Text.Json.Serialization;
using TestingService.Domains.CatalogItems.ServiceClients;
using TestingService.Domains.Listings.ServiceClients.ListingGroups;

namespace TestingService.Domains.CatalogItems.Services
{
	public class CatalogService
	{
		public Dictionary<Guid, CatalogItemInfo> CatalogItems { get; set; } = new Dictionary<Guid, CatalogItemInfo>();

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
				CatalogItems[catalogItem.Id] = catalogItem;
			}
		}
	}
}
