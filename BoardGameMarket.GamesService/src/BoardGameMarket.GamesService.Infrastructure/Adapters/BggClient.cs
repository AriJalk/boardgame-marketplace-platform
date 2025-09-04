using BoardGameMarket.GamesService.Core.Application.AdapterContracts;
using BoardGameMarket.GamesService.Core.Domain.Entities;
using BoardGameMarket.GamesService.Core.Domain.ValueObjects;
using System.Xml.Linq;

namespace BoardGameMarket.GamesService.Infrastructure.Adapters
{
	internal class BggClient : IBggClient
	{
		private HttpClient _httpClient;

		public BggClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		private IEnumerable<Category> CreateCategories(IEnumerable<XElement> categoryElements)
		{
			List<Category> categoriesList = new List<Category>();
			foreach (XElement element in categoryElements)
			{
				Category category = new Category() 
				{ 
					Id = (int?)element.Attribute("objectid") ?? 0, 
					Name = element.Value 
				};
				categoriesList.Add(category);
			}
			return categoriesList;
		}

		public async Task<IEnumerable<RawBggMetadata>?> GetMultiBggMetadataAsync(IEnumerable<int> bggIds)
		{
			string ids = string.Join(",", bggIds);

			Stream? response = await _httpClient.GetStreamAsync($"boardgame/{ids}");

			XDocument doc = XDocument.Load(response);

			IEnumerable<RawBggMetadata>? games = doc.Root?
				.Elements("boardgame")
				.Select(bg => new RawBggMetadata
				{
					BggId = (int)bg.Attribute("objectid")!,
					Name = bg.Elements("name")
					 .FirstOrDefault(x => (string?)x.Attribute("primary") == "true")?.Value
				   ?? bg.Element("name")?.Value,
					Categories = CreateCategories(bg.Elements("boardgamecategory"))
				});

			return games;
		}
	}
}
