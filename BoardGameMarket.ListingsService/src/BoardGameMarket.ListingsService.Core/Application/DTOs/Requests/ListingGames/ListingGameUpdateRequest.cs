using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingGames
{
	public record ListingGameUpdateRequest(Guid Id, Guid ListingId, Guid GameId) { }
}
