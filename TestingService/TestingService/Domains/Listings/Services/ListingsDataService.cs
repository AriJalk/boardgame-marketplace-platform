using System.Text.Json;
using System.Text.Json.Serialization;
using TestingService.Domains.Listings.Helpers;
using TestingService.Domains.Listings.ServiceClients.ListingGroups;
using TestingService.Domains.Listings.ServiceClients.Listings;
using TestingService.Domains.Users.Dtos;

namespace TestingService.Domains.Listings.Services
{
	public class ListingsDataService
	{
		private Random _random = new Random();
		public Dictionary<Guid, ListingGroupDetail> ListingGroups = new Dictionary<Guid, ListingGroupDetail>();
		public Dictionary<Guid, ListingDetail> Listings = new Dictionary<Guid, ListingDetail>();

		public Dictionary<Guid, UserSummaryDto> Users = new Dictionary<Guid, UserSummaryDto>();

		public ListingsDataService(IWebHostEnvironment hostEnvironment, JsonSerializerOptions jsonOptions)
		{
			string json = File.ReadAllText(Path.Combine(hostEnvironment.ContentRootPath, "SeedData", "LISTING_GROUPS_MOCK_DATA.json"));

			IEnumerable<ListingGroupDetail>? listingGroups = JsonSerializer.Deserialize<IEnumerable<ListingGroupDetail>>(json, jsonOptions);

			if (listingGroups == null)
				return;
			foreach (ListingGroupDetail listingGroup in listingGroups)
			{
				BuildListingGroup(listingGroup);
			}
		}

		private void BuildListingGroup(ListingGroupDetail listingGroup)
		{
			ListingGroups[listingGroup.Id] = listingGroup;
			if (listingGroup.Listings == null)
			{
				return;
			}
			foreach (ListingInGroupDetail listing in listingGroup.Listings)
			{
				Listings[listing.Id] = listing.ToListingDetail(listingGroup.Id);
			}
		}
	}
}
