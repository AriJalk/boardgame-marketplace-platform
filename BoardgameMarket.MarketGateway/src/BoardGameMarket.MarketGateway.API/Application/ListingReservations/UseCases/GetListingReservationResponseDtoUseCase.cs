using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Mappers;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Services;
using BoardGameMarket.MarketGateway.API.Application.ListingReservations.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingReservations.ServiceContracts;
using BoardGameMarket.MarketGateway.API.Application.ListingReservations.Services;
using BoardGameMarket.MarketGateway.API.Application.Listings.Mappers;
using BoardGameMarket.MarketGateway.API.Application.Listings.Services;
using BoardGameMarket.MarketGateway.API.Application.Users.Mappers;
using BoardGameMarket.MarketGateway.API.Application.Users.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;

namespace BoardGameMarket.MarketGateway.API.Application.ListingReservations.UseCases
{
    public class GetListingReservationResponseDtoUseCase
    {
        private readonly ListingsDataService _listingDataService;
        private readonly QueuesDataService _queuesDataService;
        private readonly ListingReservationDataService _listingReservationDataService;
        private readonly UsersDataService _usersDataService;


        public GetListingReservationResponseDtoUseCase(ListingsDataService listingDataService, QueuesDataService queuesDataService, ListingReservationDataService listingReservationDataService, UsersDataService usersDataService)
        {
            _listingDataService = listingDataService;
            _queuesDataService = queuesDataService;
            _listingReservationDataService = listingReservationDataService;
            _usersDataService = usersDataService;
        }

        public async Task<ListingReservationResponseDto> ExecuteAsync(Guid listingReservationId)
        {
            ListingReservationInfo reservation = await _listingReservationDataService.GetListingReservation(listingReservationId);
            UserInfo buyer = await _usersDataService.GetUser(reservation.ListingQueueEntry.BuyerUserId);
            UserInfo seller = await _usersDataService.GetUser(reservation.Listing.SellerUserId);
            return new ListingReservationResponseDto(reservation.Id, reservation.Listing.ToListingSummaryDto(seller.ToUserSummaryDto()), reservation.ListingQueueEntry.ToListingQueueEntryResponseDto(buyer.ToUserSummaryDto()), reservation.ReservedAt);
        }
    }
}
