using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Services;
using BoardGameMarket.MarketGateway.API.Application.Users.Dtos;
using BoardGameMarket.MarketGateway.API.Application.Users.Helpers;
using BoardGameMarket.MarketGateway.API.Application.Users.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;

namespace BoardGameMarket.MarketGateway.API.Application.ListingQueues.UseCases
{
	public class GetListingQueueUseCase
	{
		private readonly QueuesDataService _queueDataService;
		private readonly UsersDataService _usersDataService;

		public GetListingQueueUseCase(QueuesDataService queueDataService, UsersDataService usersDataService)
		{
			_queueDataService = queueDataService;
			_usersDataService = usersDataService;
		}

		public async Task<ListingQueueResponseDto> ExecuteAsync(Guid listingId)
		{
			ListingQueueInfo listingQueueInfo = await _queueDataService.GetQueueForListing(listingId);

			Dictionary<Guid, ListingQueueEntryResponseDto> queueByTime = new Dictionary<Guid, ListingQueueEntryResponseDto>();
			List<ListingQueueEntryResponseDto> queueByPriority = new List<ListingQueueEntryResponseDto>();

			foreach (ListingQueueEntryInfo queueEntry in listingQueueInfo.QueueByTime)
			{
				UserInfo userInfo = await _usersDataService.GetUser(queueEntry.BuyerUserId);
				UserSummaryDto userSummaryDto = userInfo.ToUserSummaryDto();

				ListingQueueEntryResponseDto queueEntryDto = new ListingQueueEntryResponseDto(
					queueEntry.Id, 
					queueEntry.Status, 
					userSummaryDto,
					queueEntry.CreatedAt,
					queueEntry.ModifiedAt, 
					queueEntry.Priority);
				queueByTime[queueEntry.Id] = queueEntryDto;
			}

			queueByPriority.AddRange(listingQueueInfo
				.QueueByPriority
				.Select(qe => queueByTime[qe.Id]));
			
			return new ListingQueueResponseDto(queueByTime.Values.ToArray(), queueByPriority.ToArray());
		}
	}
}
