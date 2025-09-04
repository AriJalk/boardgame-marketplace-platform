using BoardGameMarket.ListingsService.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingQueueEntries
{
	public record ListingQueueEntryUpdateRequest(Guid Id, Guid ListingId, QueueStatus Status, Guid BuyerUserId) { }
}
