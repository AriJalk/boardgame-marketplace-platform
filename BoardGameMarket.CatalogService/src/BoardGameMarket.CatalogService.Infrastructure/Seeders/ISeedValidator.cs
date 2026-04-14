using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.CatalogService.Infrastructure.Seeders
{
    public interface ISeedValidator
    {
        Task<bool> IsSeeded();
    }
}
