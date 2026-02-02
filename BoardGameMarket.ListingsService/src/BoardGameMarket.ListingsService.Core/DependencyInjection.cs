using BoardGameMarket.ListingsService.Core.Application.Access.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingsSold;
using BoardGameMarket.ListingsService.Core.Application.Aggregates;
using BoardGameMarket.ListingsService.Core.Application.Identity;
using BoardGameMarket.ListingsService.Core.Application.Queries;
using BoardGameMarket.ListingsService.Core.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace BoardGameMarket.ListingsService.Core
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddCore(this IServiceCollection services)
		{

			// Listing services
			services.AddTransient<IListingWriteService, ListingWriteService>();
			services.AddTransient<IListingReadService, ListingReadService>();

			// ListingGroup services
			services.AddTransient<IListingGroupReadService, ListingGroupReadService>();
			services.AddTransient<IListingGroupWriteService, ListingGroupWriteService>();

			// ListingItemInfo services
			services.AddTransient<IListingItemWriteService, ListingItemWriteService>();
			services.AddTransient<IListingItemReadService, ListingItemReadService>();

			// ListingQueueEntry services
			services.AddTransient<IListingQueueEntryWriteService, ListingQueueEntryWriteService>();
			services.AddTransient<IListingQueueEntryReadService, ListingQueueEntryReadService>();


			// ListingSold services
			services.AddTransient<IListingSoldWriteService, ListingSoldWriteService>();
			services.AddTransient<IListingSoldReadService, ListingSoldReadService>();


			// Aggregates
			services.AddScoped<ListingAggregateBuilder>();
			services.AddScoped<ListingGroupAggregateBuilder>();
			services.AddScoped<ListingQueueAggregateBuilder>();


			// Context
			services.AddScoped<ICurrentUserContext, CurrentUserContext>();

			//Use Cases
			services.AddScoped<GetQueueUseCase>();
			services.AddScoped<AddListingQueueEntryUseCase>();

			// Queries
			services.AddScoped<ListingOwnershipQuery>();
			services.AddScoped<SubscriptionQueries>();

			return services;
		}
	}
}
