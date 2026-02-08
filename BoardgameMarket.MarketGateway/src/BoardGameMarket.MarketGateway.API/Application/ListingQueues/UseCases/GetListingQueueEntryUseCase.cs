using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Assemblers;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Services;

namespace BoardGameMarket.MarketGateway.API.Application.ListingQueues.UseCases
{
	public class GetListingQueueEntryUseCase
	{
		private readonly QueuesDataService _queuesDataService;
		private readonly ListingQueueEntryDtoAssembler _queueEntryDtoAssembler;

		public GetListingQueueEntryUseCase(QueuesDataService queuesDataService, ListingQueueEntryDtoAssembler queueEntryDtoAssembler)
		{
			_queuesDataService = queuesDataService;
			_queueEntryDtoAssembler = queueEntryDtoAssembler;
		}

		public async Task<ListingQueueEntryResponseDto?> ExecuteAsync(Guid listingId, Guid userId)
		{
			ListingQueueEntryInfo? queueEntry = await _queuesDataService.GetSubscriptionForListingAsync(listingId, userId);
			if (queueEntry == null)
			{
				return null;
			}

			return await _queueEntryDtoAssembler.BuildQueueEntryResponseDto(queueEntry);
		}
	}
}
