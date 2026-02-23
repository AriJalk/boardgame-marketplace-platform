using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.CatalogService.Core.Application.Contracts
{
    public record CustomCatalogItemsRequest(string[] Names)
    {
    }
}
