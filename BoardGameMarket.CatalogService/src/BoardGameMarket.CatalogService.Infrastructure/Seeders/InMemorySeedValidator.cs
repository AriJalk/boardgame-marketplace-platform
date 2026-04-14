using BoardGameMarket.CatalogService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.CatalogService.Infrastructure.Seeders
{
    internal class InMemorySeedValidator : ISeedValidator
    {
        private readonly ICatalogItemsRepository repo;
        
        public InMemorySeedValidator(ICatalogItemsRepository repo)
        {
            this.repo = repo;
        }
        public async Task<bool> IsSeeded()
        {
            return (await this.repo.GetCatalogItemsAsync()).Any();
        }
    }
}
