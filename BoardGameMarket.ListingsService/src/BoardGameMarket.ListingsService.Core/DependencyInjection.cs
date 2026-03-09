using BoardGameMarket.ListingsService.Core.Application.Access.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingReservations;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingSales;
using BoardGameMarket.ListingsService.Core.Application.Aggregates;
using BoardGameMarket.ListingsService.Core.Application.Identity;
using BoardGameMarket.ListingsService.Core.Application.Queries;
using BoardGameMarket.ListingsService.Core.Application.UseCases.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.UseCases.ListingQueue;
using BoardGameMarket.ListingsService.Core.Application.UseCases.ListingReservations;
using Microsoft.Extensions.DependencyInjection;

namespace BoardGameMarket.ListingsService.Core
{
	public static class DependencyInjection
	{
		private static IServiceCollection AddListingQueueUseCases(this IServiceCollection services)
		{
			services.AddScoped<AddListingQueueEntryPriorityUseCase>();
			services.AddScoped<AddListingQueueEntryUseCase>();
			services.AddScoped<DeleteListingQueueEntryUseCase>();
			services.AddScoped<GetListingQueueEntryByIdUseCase>();
			services.AddScoped<GetListingQueueEntryByUserIdUseCase>();
			services.AddScoped<GetQueueUseCase>();
			services.AddScoped<RemovePriorityFromQueueEntryUseCase>();
			services.AddScoped<UpdateListingQueueEntryPriorityUseCase>();
			services.AddScoped<ReserveListingToQueueEntryUseCase>();

			return services;
        }
		public static IServiceCollection AddCore(this IServiceCollection services)
		{

			// Listing services
			services.AddScoped<IListingWriteService, ListingWriteService>();
			services.AddScoped<IListingReadService, ListingReadService>();

			// ListingGroup services
			services.AddScoped<IListingGroupReadService, ListingGroupReadService>();
			services.AddScoped<IListingGroupWriteService, ListingGroupWriteService>();

			// ListingItemInfo services
			services.AddScoped<IListingItemWriteService, ListingItemWriteService>();
			services.AddScoped<IListingItemReadService, ListingItemReadService>();

			// ListingQueueEntry services
			services.AddScoped<IListingQueueEntryWriteService, ListingQueueEntryWriteService>();
			services.AddScoped<IListingQueueEntryReadService, ListingQueueEntryReadService>();


			// ListingSale services
			services.AddScoped<IListingSaleWriteService, ListingSaleWriteService>();
			services.AddScoped<IListingSaleReadService, ListingSaleReadService>();

			// ListingReservation services
			services.AddScoped<IListingReservationReadService, ListingReservationReadService>();
			services.AddScoped<IListingReservationWriteService, ListingReservationWriteService>();


			// Aggregates
			services.AddScoped<ListingAggregateBuilder>();
			services.AddScoped<ListingGroupAggregateBuilder>();
			services.AddScoped<ListingQueueAggregateBuilder>();


			// Context
			services.AddScoped<ICurrentUserContext, CurrentUserContext>();

			//Use Cases
			services.AddScoped<AddListingGroupUseCase>();
			services.AddScoped<UpdateListingGroupUseCase>();
			services.AddScoped<GetListingReservationUseCase>();
			services.AddScoped<GetListingReservationsUseCase>();
			services.AddScoped<ApproveReservationSellUseCase>();
			services.AddScoped<CancelReservationUseCase>();

			AddListingQueueUseCases(services);

			// Queries
			services.AddScoped<SubscriptionQueries>();

			return services;
		}
	}
}
