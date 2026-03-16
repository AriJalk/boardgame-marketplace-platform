using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Infrastructure.Helpers
{
    internal static class DapperHelper
    {
        public static object Param(string name, object value) => new Dictionary<string, object> { [name] = value };

        public static object Params(Dictionary<string, object> parameters) => parameters;
    }
}
