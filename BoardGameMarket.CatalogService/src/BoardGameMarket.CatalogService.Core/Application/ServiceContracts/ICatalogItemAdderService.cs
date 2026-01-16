using BoardGameMarket.CatalogService.Core.Application.Commands;
using BoardGameMarket.CatalogService.Core.Domain.Entities;
using BoardGameMarket.CatalogService.Core.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.CatalogService.Core.Application.ServiceContracts
{
	public interface ICatalogItemAdderService
	{
		Task<CatalogItem?> AddBggCatalogItemAsync(AddBggCatalogItemCommand addBggCatalogItemCommand);
		Task<CatalogItem?> AddCustomCatalogItemAsync(AddCustomCatalogItemCommand addCustomCatalogItemCommand);
	}
}
