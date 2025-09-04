using BoardGameMarket.GamesService.Core.Application.AdapterContracts;
using BoardGameMarket.GamesService.Core.Application.ServiceContracts;
using BoardGameMarket.GamesService.Core.Domain.Entities;
using BoardGameMarket.GamesService.Core.Domain.RepositoryContracts;
using BoardGameMarket.GamesService.Core.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.GamesService.Core.Application.Services
{
	public class BggItemGetterService : IBggItemGetterService
	{
		private readonly IBggItemsRepository _bggItemsRepository;
		private readonly ICategoriesRepository _categoriesRepository;
		private readonly IBggClient _bggClient;
		private readonly ILogger<BggItemGetterService> _logger;
		private readonly IBggItemFactory _bggItemFactory;

		public BggItemGetterService(IBggItemsRepository bggItemsRepository, ICategoriesRepository categoriesRepository, IBggClient bggClient, ILogger<BggItemGetterService> logger, IBggItemFactory bggItemFactory)
		{
			_bggItemsRepository = bggItemsRepository;
			_categoriesRepository = categoriesRepository;
			_bggClient = bggClient;
			_logger = logger;
			_bggItemFactory = bggItemFactory;
		}

		private async Task<BggMetadata> CreateMetadataFromRaw(RawBggMetadata rawBggMetadata)
		{
			IEnumerable<Task<int>> categoryTasks = rawBggMetadata.Categories.Select(async category =>
			{
				if (!await _categoriesRepository.ExistsAsync(category.Id))
					await _categoriesRepository.AddCategoryAsync(category);
				return category.Id;
			});
			var categories = (await Task.WhenAll(categoryTasks)).ToList();

			return new BggMetadata() { BggId = rawBggMetadata.BggId, Name = rawBggMetadata.Name, Categories = categories };
		}

		private async Task<IEnumerable<BggItem>> AddItemsToDatabase(IEnumerable<RawBggMetadata> rawMetadataCollection)
		{
			List<BggItem> bggItems = new List<BggItem>();
			foreach (RawBggMetadata rawMetadata in rawMetadataCollection)
			{
				BggItem? bggItem = await _bggItemsRepository.GetItemByBggIdAsync(rawMetadata.BggId);
				if (bggItem == null)
				{
					_logger.LogInformation($"{nameof(BggItemGetterService)}:{nameof(AddItemsToDatabase)}: Adding bggItem to database");
					BggMetadata metadata = await CreateMetadataFromRaw(rawMetadata);
					bggItem = _bggItemFactory.CreateFromMetadata(metadata);
					await _bggItemsRepository.AddBggItemToDatabaseAsync(bggItem);
				}
				bggItems.Add(bggItem);

			}
			return bggItems;
		}

		public async Task<IEnumerable<BggItem>> GetItemsByBggIds(IEnumerable<int> bggIds)
		{
			List<BggItem> existingItems = new List<BggItem>();
			HashSet<int> itemsToFetch = new HashSet<int>();
			foreach (int id in bggIds)
			{
				BggItem? bggItem = await _bggItemsRepository.GetItemByBggIdAsync(id);
				if (bggItem != null)
				{
					existingItems.Add(bggItem);
					continue;
				}
				_logger.LogInformation($"{nameof(BggItemGetterService)}:{nameof(GetItemsByBggIds)}: Item not found in database, will attempt to fetch from BGG");
				itemsToFetch.Add(id);
			}
			if (itemsToFetch.Count > 0)
			{
				_logger.LogInformation($"{nameof(BggItemGetterService)}:{nameof(GetItemsByBggIds)}: Batch fetching missing bggItems");
				IEnumerable<RawBggMetadata>? rawMetadataCollection = await _bggClient.GetMultiBggMetadataAsync(itemsToFetch);
				if (rawMetadataCollection != null)
				{
					existingItems.AddRange(await AddItemsToDatabase(rawMetadataCollection));
				}
			}
			return existingItems;
		}

		public async Task<BggItem?> GetItemByDatabaseId(Guid id)
		{
			return await _bggItemsRepository.GetItemByDatabaseIdAsync(id);
		}
	}
}
