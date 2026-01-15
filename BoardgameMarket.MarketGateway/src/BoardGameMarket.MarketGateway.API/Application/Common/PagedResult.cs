using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.MarketGateway.API.Application.Common
{
	public class PagedResult<T>
	{
		public T[] Items { get; set; } = Array.Empty<T>();
		public string? NextCursor { get; set; }
	}
}
