using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Infrastructure.InMemoryRepositories
{
    internal class InMemoryListingReservationRepository : IListingReservationRepository
    {
        private readonly List<ListingReservation> _listingReservations = new List<ListingReservation>();
        public Task<int> AddListingReservationAsync(ListingReservation listingReservation)
        {
            _listingReservations.Add(listingReservation);
            return Task.FromResult(1);
        }

        public Task<int> DeleteListingReservationAsync(Guid listingReservationId)
        {
            int index = _listingReservations.FindIndex(lr => lr.Id == listingReservationId);
            if (index == -1)
            {
                return Task.FromResult(0);
            }
            _listingReservations.RemoveAt(index);
            return Task.FromResult(1);
        }

        public Task<ListingReservation?> GetListingReservationByIdAsync(Guid listingReservationId)
        {
            return Task.FromResult(_listingReservations.FirstOrDefault(lr => lr.Id == listingReservationId));
        }

        public Task<ListingReservation?> GetListingReservationByListingIdAsync(Guid listingId)
        {
            return Task.FromResult(_listingReservations.FirstOrDefault(lr => lr.ListingId == listingId));
        }

        public Task<IEnumerable<ListingReservation>> GetListingsReservationsByBuyerUserIdAsync(Guid buyerUserId)
        {
            return Task.FromResult(_listingReservations.Where(lr => lr.BuyerUserId == buyerUserId));
        }

        public Task<IEnumerable<ListingReservation>> GetListingsReservationsBySellerUserIdAsync(Guid sellerUserId)
        {
            return Task.FromResult(_listingReservations.Where(lr => lr.SellerUserId == sellerUserId));
        }

        public Task<int> UpdateListingReservationAsync(ListingReservation listingReservation)
        {
            int index = _listingReservations.FindIndex(lr => lr.Id == listingReservation.Id);
            if (index == -1)
            {
                return Task.FromResult(0);
            }
            _listingReservations[index] = listingReservation;
            return Task.FromResult(1);
        }
    }
}
