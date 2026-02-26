using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Mappers;
using BoardGameMarket.MarketGateway.API.Application.ListingReservations.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingReservations.ServiceContracts;
using BoardGameMarket.MarketGateway.API.Application.ListingReservations.Services;
using BoardGameMarket.MarketGateway.API.Application.Listings.Mappers;
using BoardGameMarket.MarketGateway.API.Application.Users.Dtos;
using BoardGameMarket.MarketGateway.API.Application.Users.Mappers;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;

namespace BoardGameMarket.MarketGateway.API.Application.ListingReservations.UseCases
{
    public class GetCurrentUserReservationsUseCase
    {
        private GetListingReservationResponseDtoUseCase _getListingReservationResponseDtoUseCase;
        private ListingReservationDataService _listingReservationDataService;
        private UsersDataService _usersDataService;

        public GetCurrentUserReservationsUseCase(GetListingReservationResponseDtoUseCase getListingReservationResponseDtoUseCase, ListingReservationDataService listingReservationDataService, UsersDataService usersDataService)
        {
            _getListingReservationResponseDtoUseCase = getListingReservationResponseDtoUseCase;
            _listingReservationDataService = listingReservationDataService;
            _usersDataService = usersDataService;
        }

        public async Task<IEnumerable<ListingReservationResponseDto>> ExecuteAsync()
        {
            IEnumerable<ListingReservationInfo> reservations = await _listingReservationDataService.GetCurrentUserListingReservations();

            List<ListingReservationResponseDto> dtos = new List<ListingReservationResponseDto>();
            Dictionary<Guid, UserSummaryDto> userDtos = new Dictionary<Guid, UserSummaryDto>();
            foreach (ListingReservationInfo reservation in reservations)
            {
                Guid SellerUserId = reservation.Listing.SellerUserId;
                Guid BuyerUserId = reservation.ListingQueueEntry.BuyerUserId;
                if (!userDtos.TryGetValue(SellerUserId, out var existingUser))
                {
                    existingUser = (await _usersDataService.GetUser(SellerUserId)).ToUserSummaryDto();
                    userDtos[existingUser.UserId] = existingUser;
                }
                if (!userDtos.TryGetValue(BuyerUserId, out existingUser))
                {
                    existingUser = (await _usersDataService.GetUser(BuyerUserId)).ToUserSummaryDto();
                    userDtos[existingUser.UserId] = existingUser;
                }
                ListingReservationResponseDto dto = new ListingReservationResponseDto(reservation.Id, reservation.Listing.ToListingSummaryDto(userDtos[reservation.Listing.SellerUserId]), reservation.ListingQueueEntry.ToListingQueueEntryResponseDto(userDtos[reservation.ListingQueueEntry.BuyerUserId]), reservation.ReservedAt);
                dtos.Add(dto);
            }
            return dtos;
        }
    }
}
