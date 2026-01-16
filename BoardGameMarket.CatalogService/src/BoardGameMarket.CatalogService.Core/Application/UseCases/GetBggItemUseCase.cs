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

namespace BoardGameMarket.CatalogService.Core.Application.UseCases
{
	public class GetBggItemUseCase
	{
		private readonly ICatalogItemGetterService _catalogItemGetterService;
		private readonly ICatalogItemAdderService _catalogItemAdderService;
		private readonly IBggClient _bggClient;

		public GetBggItemUseCase(ICatalogItemGetterService catalogItemGetterService, ICatalogItemAdderService catalogItemAdderService, IBggClient bggClient)
		{
			_catalogItemGetterService = catalogItemGetterService;
			_catalogItemAdderService = catalogItemAdderService;
			_bggClient = bggClient;
		}

		public async Task<CatalogItemInfo> Execute(int bggId)
		{
			CatalogItem? catalogItem = await _catalogItemGetterService.GetBggItemByBggIdAsync(bggId);
			// Fetch data from bgg
			if (catalogItem == null)
			{
				RawBggMetadata? metadata = (await _bggClient.GetMultiBggMetadataAsync([bggId])).FirstOrDefault();

				if (metadata == null)
				{
					throw new KeyNotFoundException();
				}
				catalogItem = await _catalogItemAdderService.AddBggCatalogItemAsync(new AddBggCatalogItemCommand(metadata));
				if (catalogItem == null)
				{
					throw new InvalidOperationException();
				}
			}
			return catalogItem.ToCatalogItemInfo();
		}
	}
}
