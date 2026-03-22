using BoardGameMarket.CatalogService.Core.Application.AdapterContracts;
using BoardGameMarket.CatalogService.Core.Application.ServiceContracts;
using BoardGameMarket.CatalogService.Core.Domain.Entities;
using BoardGameMarket.CatalogService.Core.Domain.ValueObjects;
using BoardGameMarket.CatalogService.Core.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using BoardGameMarket.CatalogService.Core.Application.Mappers;
using BoardGameMarket.CatalogService.Core.Application.Commands;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.CatalogService.Core.Application.UseCases
{
	public class GetBggItemUseCase
	{
		private readonly ICatalogItemGetterService _catalogItemGetterService;
		private readonly ICatalogItemAdderService _catalogItemAdderService;
		private readonly IBggClient _bggClient;
		private readonly ILogger<GetBggItemUseCase> _logger;

		public GetBggItemUseCase(ICatalogItemGetterService catalogItemGetterService, ICatalogItemAdderService catalogItemAdderService, IBggClient bggClient, ILogger<GetBggItemUseCase> logger)
		{
			_catalogItemGetterService = catalogItemGetterService;
			_catalogItemAdderService = catalogItemAdderService;
			_bggClient = bggClient;
			_logger = logger;
		}

		public async Task<CatalogItemInfo> ExecuteAsync(int bggId)
		{
			CatalogItem? catalogItem = await _catalogItemGetterService.GetBggItemByBggIdAsync(bggId);
			// Fetch data from bgg
			if (catalogItem == null)
			{
				RawBggMetadata? metadata = (await _bggClient.GetMultiBggMetadataAsync([bggId])).FirstOrDefault();

				if (metadata == null)
				{
					_logger.LogError("{Service}/{Method}: BGG item {BggId} not found on BGG", nameof(GetBggItemUseCase), nameof(ExecuteAsync), bggId);
					throw new KeyNotFoundException();
				}
				catalogItem = await _catalogItemAdderService.AddBggCatalogItemAsync(new AddBggCatalogItemCommand(metadata));
				if (catalogItem == null)
				{
                    _logger.LogError("{Service}/{Method}: Can't add {BggId} to db", nameof(GetBggItemUseCase), nameof(ExecuteAsync), bggId);
                    throw new InvalidOperationException();
				}
			}
			return catalogItem.ToCatalogItemInfo();
		}
	}
}
