using BoardGameMarket.CatalogService.Core.Application.Commands;
using BoardGameMarket.CatalogService.Core.Application.Contracts;
using BoardGameMarket.CatalogService.Core.Application.Mappers;
using BoardGameMarket.CatalogService.Core.Application.ServiceContracts;
using BoardGameMarket.CatalogService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.CatalogService.Core.Application.UseCases
{
    public class GetCustomItemsUseCase
    {
        private ICatalogItemGetterService _catalogItemGetterService;
        private ICatalogItemAdderService _catalogItemAdderService;

        public GetCustomItemsUseCase(ICatalogItemGetterService catalogItemGetterService, ICatalogItemAdderService catalogItemAdderService)
        {
            _catalogItemGetterService = catalogItemGetterService;
            _catalogItemAdderService = catalogItemAdderService;
        }

        public async Task<IReadOnlyCollection<CatalogItemInfo>> Execute(string[] names)
        {
            IEnumerable<CatalogItem> catalogItems = await _catalogItemGetterService.GetCustomItemsByNamesAsync(names);
            IEnumerable<string> matchingNames = catalogItems.Select(i => i.Name);

            List<string> namesToAdd = names.Where(n => !matchingNames.Contains(n)).ToList();
            foreach (string nameToAdd in namesToAdd)
            {
                CatalogItem? item = await _catalogItemAdderService.AddCustomCatalogItemAsync(new AddCustomCatalogItemCommand(nameToAdd));
                if (item != null)
                {
                    catalogItems.Append(item);
                }
            }

            return catalogItems.Select(i => i.ToCatalogItemInfo()).ToList();
        }
    }
}
