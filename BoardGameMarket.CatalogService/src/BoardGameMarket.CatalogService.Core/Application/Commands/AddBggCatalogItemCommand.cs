using BoardGameMarket.CatalogService.Core.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.CatalogService.Core.Application.Commands
{
	public record AddBggCatalogItemCommand(RawBggMetadata Metadata)
	{
	}
}
