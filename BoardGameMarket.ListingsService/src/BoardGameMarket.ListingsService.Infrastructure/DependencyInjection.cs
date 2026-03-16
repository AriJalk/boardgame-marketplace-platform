using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using BoardGameMarket.ListingsService.Infrastructure.InMemoryRepositories;
using BoardGameMarket.ListingsService.Infrastructure.Repositories;
using BoardGameMarket.ListingsService.Infrastructure.Repositories.DbContext;
using BoardGameMarket.ListingsService.Infrastructure.Seeders;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Polly;
using Polly.Retry;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BoardGameMarket.ListingsService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            services.AddScoped<PostgresDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IListingGroupRepository, ListingGroupRepository>();
            services.AddScoped<IListingRepository, ListingRepository>();
            services.AddScoped<IListingItemRepository, ListingItemRepository>();
            services.AddScoped<IListingQueueEntryRepository, ListingQueueEntryRepository>();
            services.AddScoped<IListingSaleRepository, ListingSaleRepository>();
            services.AddScoped<IListingReservationRepository, ListingReservationRepository>();

            services.AddSingleton<AsyncRetryPolicy>(sp =>
            Policy.Handle<NpgsqlException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: _ => TimeSpan.FromSeconds(5),
                onRetry: (exception, timespan, retryCount, context) =>
                {
                    Console.WriteLine($"Retry {retryCount} after {timespan.TotalSeconds}s due to {exception.Message}");
                }));

            return services;
        }

        public static IServiceCollection AddInMemoryInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, InMemoryUnitOfWork>();
            services.AddSingleton<IListingGroupRepository, InMemoryListingGroupRepository>();
            //services.AddScoped<IListingGroupRepository, ListingGroupRepository>
            services.AddSingleton<IListingRepository, InMemoryListingRepository>();
            services.AddSingleton<IListingItemRepository, InMemoryListingItemRepository>();
            services.AddSingleton<IListingQueueEntryRepository, InMemoryListingQueueEntryRepository>();
            services.AddSingleton<IListingSaleRepository, InMemoryListingSaleRepository>();
            services.AddSingleton<IListingReservationRepository, InMemoryListingReservationRepository>();

            return services;
        }

        public static IServiceCollection AddSeeding(this IServiceCollection services)
        {
            JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
            jsonOptions.PropertyNameCaseInsensitive = true;
            jsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            services.AddSingleton(jsonOptions);

            services.AddTransient<SeedRunner>();
            services.AddTransient<ISeeder, ListingGroupsSeeder>();
            services.AddTransient<ISeeder, ListingsSeeder>();
            services.AddTransient<ISeeder, ListingItemsSeeder>();
            services.AddTransient<QueueSeeder>();

            return services;
        }
    }
}
