using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Mappers
{
	internal static class ListingGameMappers
	{
		public static ListingGameResponseDto ToDto(this ListingGame listingGame)
		{
			return new ListingGameResponseDto()
			{
				Id = listingGame.Id,
				GameId = listingGame.GameId,
			};
		}
	}
}
