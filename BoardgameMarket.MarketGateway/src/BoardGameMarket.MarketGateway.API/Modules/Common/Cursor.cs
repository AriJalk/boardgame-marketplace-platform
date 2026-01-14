using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.MarketGateway.API.Modules.Common
{
	public record Cursor(DateTime LastCreatedAt, Guid LastId) { }
}
