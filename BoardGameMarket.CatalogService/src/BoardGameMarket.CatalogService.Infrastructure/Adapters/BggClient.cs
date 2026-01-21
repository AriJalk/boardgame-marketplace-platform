using BoardGameMarket.CatalogService.Core.Application.AdapterContracts;
using BoardGameMarket.CatalogService.Core.Domain.Entities;
using BoardGameMarket.CatalogService.Core.Domain.ValueObjects;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Xml.Linq;

namespace BoardGameMarket.CatalogService.Infrastructure.Adapters
{
	internal class BggClient : IBggClient
	{
		private readonly HttpClient _httpClient;

		public BggClient(HttpClient httpClient, IOptions<BggClientConfiguration> configuration)
		{
			_httpClient = httpClient;
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuration.Value.ServiceApiKey);
		}

		private IReadOnlyCollection<string> CreateCategories(IEnumerable<XElement> categoryElements)
		{
			List<string> categoriesList = new List<string>();
			foreach (XElement element in categoryElements)
			{
				categoriesList.Add(element.Value);
			}
			return categoriesList;
		}

		public async Task<IEnumerable<RawBggMetadata>> GetMultiBggMetadataAsync(IEnumerable<int> bggIds)
		{
			string ids = string.Join(",", bggIds);

			Stream? response = await _httpClient.GetStreamAsync($"boardgame/{ids}");

			XDocument doc = XDocument.Load(response);

			IEnumerable<RawBggMetadata>? games = doc.Root?
				.Elements("boardgame")
				.Select(bg => new RawBggMetadata(
					BggId: (int)bg.Attribute("objectid")!,
					Name: bg.Elements("name")
					 .FirstOrDefault(x => (string?)x.Attribute("primary") == "true")?.Value
				   ?? bg.Element("name")?.Value ?? "",
					Categories: CreateCategories(bg.Elements("boardgamecategory"))
				));
			if (games == null)
			{
				throw new InvalidOperationException();
			}

			return games;
		}
	}
}
