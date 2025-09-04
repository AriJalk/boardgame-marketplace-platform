using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Mappers
{
	internal static class ListingQueueEntryMappers
	{
		public static ListingQueueEntryResponseDto ToDto(this ListingQueueEntry listingQueueEntry)
		{
			return new ListingQueueEntryResponseDto()
			{
				Id = listingQueueEntry.Id,
				BuyerUserId = listingQueueEntry.BuyerUserId,
				CreatedAt = listingQueueEntry.CreatedAt,
				Status = listingQueueEntry.Status,
			};
		}
	}
}
