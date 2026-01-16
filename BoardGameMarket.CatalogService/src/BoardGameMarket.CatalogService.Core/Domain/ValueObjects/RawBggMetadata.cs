using BoardGameMarket.CatalogService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.CatalogService.Core.Domain.ValueObjects
{
	/// <summary>
	/// Used for data fetched from BGG
	/// </summary>
	public record RawBggMetadata(int BggId, string Name, IReadOnlyCollection<string>? Categories = null)
	{
	}
}
