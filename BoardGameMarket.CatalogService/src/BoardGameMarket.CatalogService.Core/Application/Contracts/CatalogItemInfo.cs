using BoardGameMarket.CatalogService.Core.Domain.Entities;
using BoardGameMarket.CatalogService.Core.Domain.Enums;
using BoardGameMarket.CatalogService.Core.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.CatalogService.Core.Application.Contracts
{
	public record CatalogItemInfo(Guid Id, CatalogItemType Type, string Name, string? BggUrl, string? ImageUrl) { }
}
