using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Mappers;
using BoardGameMarket.MarketGateway.API.Application.ListingGroups.Services;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Mappers;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Services;
using BoardGameMarket.MarketGateway.API.Application.ListingReservations.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingReservations.ServiceContracts;
using BoardGameMarket.MarketGateway.API.Application.ListingReservations.Services;
using BoardGameMarket.MarketGateway.API.Application.Listings.Mappers;
using BoardGameMarket.MarketGateway.API.Application.Listings.Services;
using BoardGameMarket.MarketGateway.API.Application.Users.Dtos;
using BoardGameMarket.MarketGateway.API.Application.Users.Mappers;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;

namespace BoardGameMarket.MarketGateway.API.Application.ListingReservations.UseCases
{
    public class GetListingReservationResponseDtoUseCase
    {
        private readonly ListingReservationDataService _listingReservationDataService;
        private readonly UsersDataService _usersDataService;
        private readonly ListingGroupsDataService _listingGroupsDataService;

        public GetListingReservationResponseDtoUseCase(ListingReservationDataService listingReservationDataService, UsersDataService usersDataService, ListingGroupsDataService listingGroupsDataService)
        {
            _listingReservationDataService=listingReservationDataService;
            _usersDataService=usersDataService;
            _listingGroupsDataService=listingGroupsDataService;
        }

        public async Task<ListingReservationResponseDto> ExecuteAsync(Guid listingReservationId)
        {
            ListingReservationInfo reservation = await _listingReservationDataService.GetListingReservation(listingReservationId);
            var buyerTask = _usersDataService.GetUser(reservation.ListingQueueEntry.BuyerUserId);
            var sellerTask = _usersDataService.GetUser(reservation.Listing.SellerUserId);
            var metadataTask = _listingGroupsDataService.GetListingGroupMetadata(reservation.Listing.ListingGroupId);

            await Task.WhenAll(buyerTask, sellerTask, metadataTask);

            UserSummaryDto buyer = (await buyerTask).ToUserSummaryDto();
            UserSummaryDto seller = (await sellerTask).ToUserSummaryDto();
            ListingGroupMetadataDto metadata = (await metadataTask).ToListingGroupMetadataDto(seller);

            return new ListingReservationResponseDto(reservation.Id, reservation.Listing.ToListingSummaryDto(seller), reservation.ListingQueueEntry.ToListingQueueEntryResponseDto(buyer), reservation.ReservedAt, metadata, reservation.Status);
        }
    }
}
