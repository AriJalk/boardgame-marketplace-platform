using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Pagination
{
	public class PagedResult<T>
	{
		public T[] Items { get; set; } = Array.Empty<T>();
		public string? NextCursor { get; set; }
	}
}
