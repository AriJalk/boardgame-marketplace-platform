using BoardGameMarket.BggCatalogService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.BggCatalogService.Core.Domain.ValueObjects
{
	/// <summary>
	/// Used for data fetched from BGG
	/// </summary>
	public class RawBggMetadata
	{
		public int BggId { get; set; }
		public string? Name { get; set; }
		public IEnumerable<Category> Categories { get; set; } = new List<Category>();
	}
}
