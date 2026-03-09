using BoardGameMarket.ListingsService.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Domain.Entities
{
    public class ListingReservation
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public ReservationStatus Status { get; set; }
        public Guid ListingQueueEntryId { get; set; }
        public Guid ListingId { get; set; } // Duplication
        public Guid SellerUserId { get; set; } // Duplication for fast lookup
        public Guid BuyerUserId { get; set; } // Duplication for fast lookup
    }
}
