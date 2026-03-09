using BoardGameMarket.ListingsService.Core.Application.Contracts;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Mappers
{
    public static class ListingSoldMappers
    {
        public static ListingSoldInfo ToListingSoldInfo(this ListingSold listingSold)
        {
            return new ListingSoldInfo(listingSold.Id, listingSold.ListingId, listingSold.CompletedAt, listingSold.ReservationId, listingSold.ListingQueueEntryId, listingSold.SellerUserId, listingSold.BuyerUserId);
        }
    }
}
