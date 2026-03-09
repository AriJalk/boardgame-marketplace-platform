using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingSale;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Mappers
{
    public static class ListingSaleMappers
    {
        public static ListingSaleInfo ToListingSaleInfo(this ListingSale listingSale)
        {
            return new ListingSaleInfo(listingSale.Id, listingSale.ListingId, listingSale.CompletedAt, listingSale.ReservationId, listingSale.ListingQueueEntryId, listingSale.SellerUserId, listingSale.BuyerUserId);
        }
    }
}
