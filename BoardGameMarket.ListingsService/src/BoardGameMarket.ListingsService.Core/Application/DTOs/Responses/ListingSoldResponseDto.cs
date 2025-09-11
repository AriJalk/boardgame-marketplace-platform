using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.DTOs.Responses
{
	internal class ListingSoldResponseDto
	{
		public Guid Id { get; set; }
		public ListingResponseDto? Listing {  get; set; }
		public ListingQueueEntryResponseDto? QueueEntry { get; set; }
		public DateTime CompletedAt { get; set; }
	}
}
