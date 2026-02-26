using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Domain.Entities
{
    public class ListingReservation
    {
        public Guid Id { get; set; }
        public Guid ListingId { get; set; }
        public Guid ListingQueueEntryId { get; set; }
        public DateTime ReservedAt { get; set; }
        public Guid SellerId { get; set; } // Duplication for fast lookup
        public Guid BuyerId { get; set; } // Duplication for fast lookup
    }
}
