using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Assemblers;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Dtos;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Services;

namespace BoardGameMarket.MarketGateway.API.Application.ListingQueues.UseCases
{
	public class GetListingQueueUseCase
	{
		private readonly QueuesDataService _queueDataService;
		private readonly ListingQueueEntryDtoAssembler _queueEntryDtoAssembler;

		public GetListingQueueUseCase(QueuesDataService queueDataService, ListingQueueEntryDtoAssembler queueEntryDtoAssembler)
		{
			_queueDataService = queueDataService;
			_queueEntryDtoAssembler = queueEntryDtoAssembler;
		}

		public async Task<ListingQueueResponseDto> ExecuteAsync(Guid listingId)
		{
			ListingQueueInfo listingQueueInfo = await _queueDataService.GetQueueForListing(listingId);

			Dictionary<Guid, ListingQueueEntryResponseDto> queueByTime = new Dictionary<Guid, ListingQueueEntryResponseDto>();
			List<ListingQueueEntryResponseDto> queueByPriority = new List<ListingQueueEntryResponseDto>();

			foreach (ListingQueueEntryInfo queueEntry in listingQueueInfo.QueueByTime)
			{
				ListingQueueEntryResponseDto queueEntryDto = await _queueEntryDtoAssembler.BuildQueueEntryResponseDto(queueEntry);
				queueByTime[queueEntry.Id] = queueEntryDto;
			}

			queueByPriority.AddRange(listingQueueInfo
				.QueueByPriority
				.Select(qe => queueByTime[qe.Id]));
			
			return new ListingQueueResponseDto(queueByTime.Values.ToArray(), queueByPriority.ToArray());
		}
	}
}
