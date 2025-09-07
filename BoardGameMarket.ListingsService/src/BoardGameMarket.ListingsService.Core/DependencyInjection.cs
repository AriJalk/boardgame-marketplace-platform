using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingsSold;
using BoardGameMarket.ListingsService.Core.Application.Services.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Services.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.Services.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Services.Listings;
using BoardGameMarket.ListingsService.Core.Application.Services.ListingsSold;
using Microsoft.Extensions.DependencyInjection;

namespace BoardGameMarket.ListingsService.Core
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddCore(this IServiceCollection services)
		{
			// Listing services
			services.AddTransient<IListingAdderService, ListingAdderService>();
			services.AddTransient<IListingGetterService, ListingGetterService>();
			services.AddTransient<IListingUpdaterService, ListingUpdaterService>();
			services.AddTransient<IListingDeleterService, ListingDeleterService>();

			// ListingGroup services
			services.AddTransient<IListingGroupAdderService, ListingGroupAdderService>();
			services.AddTransient<IListingGroupGetterService, ListingGroupGetterService>();
			services.AddTransient<IListingGroupUpdaterService, ListingGroupUpdaterService>();
			services.AddTransient<IListingGroupDeleterService, ListingGroupDeleterService>();

			// ListingItem services
			services.AddTransient<IListingItemAdderService, ListingItemAdderService>();
			services.AddTransient<IListingItemGetterService, ListingItemGetterService>();
			services.AddTransient<IListingItemDeleterService, ListingItemDeleterService>();

			// ListingQueueEntry services
			services.AddTransient<IListingQueueEntryAdderService, ListingQueueEntryAdderService>();
			services.AddTransient<IListingQueueEntryGetterService, ListingQueueEntryGetterService>();
			services.AddTransient<IListingQueueEntryUpdaterService, ListingQueueEntryUpdaterService>();
			services.AddTransient<IListingQueueEntryDeleterService, ListingQueueEntryDeleterService>();

			// ListingSold services
			services.AddTransient<IListingSoldAdderService, ListingSoldAdderService>();
			services.AddTransient<IListingSoldGetterService, ListingSoldGetterService>();
			services.AddTransient<IListingSoldDeleterService, ListingSoldDeleterService>();

			return services;
		}
	}
}
