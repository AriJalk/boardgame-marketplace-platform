using BoardGameMarket.MarketGateway.API.Application.Common;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Filters;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.Listings;
using BoardGameMarket.MarketGateway.API.Application.Shared;
using BoardGameMarket.MarketGateway.API.Application.Users.Dtos;
using System.Collections.Specialized;
using System.Text;
using System.Text.Json;
using System.Web;

namespace BoardGameMarket.MarketGateway.API.Application.Listings.Services
{
	public class ListingsDataService
	{
		private readonly HttpClient _httpClient;

		public ListingsDataService(ListingsHttpClient httpClient)
		{
			_httpClient = httpClient.HttpClient;
		}

	}
}
