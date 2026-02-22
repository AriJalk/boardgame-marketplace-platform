using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Pagination
{
	public record Cursor(DateTime LastCreatedAt, Guid LastId) { }
}
