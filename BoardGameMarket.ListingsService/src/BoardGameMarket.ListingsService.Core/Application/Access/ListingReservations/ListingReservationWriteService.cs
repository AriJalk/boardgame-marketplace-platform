using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingReservations
{
    internal class ListingReservationWriteService : IListingReservationWriteService
    {
        private readonly IListingReservationRepository _listingReservationRepository;

        public ListingReservationWriteService(IListingReservationRepository listingReservationRepository)
        {
            _listingReservationRepository = listingReservationRepository;
        }

        public async Task AddListingReservationAsync(ListingReservation listingReservationToAdd)
        {
            int rows = await _listingReservationRepository.AddListingReservationAsync(listingReservationToAdd);
            if (rows == 0)
            {
                throw new InvalidOperationException("Can't add reservation");
            }
        }

        public async Task DeleteListingReservationAsync(Guid listingReservationId)
        {
            int rows = await _listingReservationRepository.DeleteListingReservationAsync(listingReservationId);
        }

        public async Task UpdateListingReservationAsync(ListingReservation listingReservationToUpdate)
        {
            int rows = await _listingReservationRepository.UpdateListingReservationAsync(listingReservationToUpdate);
            if (rows == 0)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
