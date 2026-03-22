using BoardGameMarket.CatalogService.Core.Application.Contracts;
using BoardGameMarket.CatalogService.Core.Application.Mappers;
using BoardGameMarket.CatalogService.Core.Application.ServiceContracts;
using BoardGameMarket.CatalogService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.CatalogService.Core.Application.UseCases
{
	public class GetCustomItemUseCase
	{
		private readonly ICatalogItemGetterService _catalogItemGetterService;
		private readonly ICatalogItemAdderService _catalogItemAdderService;

		public GetCustomItemUseCase(ICatalogItemGetterService catalogItemGetterService, ICatalogItemAdderService catalogItemAdderService)
		{
			_catalogItemGetterService = catalogItemGetterService;
			_catalogItemAdderService = catalogItemAdderService;
		}

		public async Task<CatalogItemInfo> ExecuteAsync(string name)
		{
			CatalogItem? catalogItem = await _catalogItemGetterService.GetCustomItemByNameAsync(name);
			if (catalogItem == null)
			{
				catalogItem = await _catalogItemAdderService.AddCustomCatalogItemAsync(new Commands.AddCustomCatalogItemCommand(name));
				if (catalogItem == null)
				{
					throw new InvalidOperationException();
				}
			}

			return catalogItem.ToCatalogItemInfo();
		}
	}
}
