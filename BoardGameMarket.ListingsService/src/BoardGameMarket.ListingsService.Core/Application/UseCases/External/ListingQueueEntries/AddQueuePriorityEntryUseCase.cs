using BoardGameMarket.ListingsService.Core.Application.Identity;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueuePriorityEntries;
using BoardGameMarket.ListingsService.Core.Application.Services.Helpers;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Services;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.External.ListingQueueEntries
{
	internal class AddQueuePriorityEntryUseCase
	{
		private readonly ListingFetcher _listingFetcher;
		private readonly IListingQueuePriorityEntryAdderService _listingQueuePriorityEntryAdderService;
		private readonly PriorityQueueDomainService _priorityQueueDomainService;
		private readonly ICurrentUserContext _currentUserContext;

		public AddQueuePriorityEntryUseCase(ListingFetcher listingFetcher, IListingQueuePriorityEntryAdderService listingQueuePriorityEntryAdderService, PriorityQueueDomainService priorityQueueDomainService, ICurrentUserContext currentUserContext)
		{
			_listingFetcher = listingFetcher;
			_listingQueuePriorityEntryAdderService = listingQueuePriorityEntryAdderService;
			_priorityQueueDomainService = priorityQueueDomainService;
			_currentUserContext = currentUserContext;
		}

		public async Task<ListingQueuePriorityEntry> ExecuteAsync(Guid listingQueueEntryId, decimal priorityWeight)
		{
			ListingQueueEntry listingQueueEntry = await _listingFetcher.GetListingQueueEntryAsync(listingQueueEntryId);

			//Authorize
			Guid sellerId = await _listingFetcher.GetSellerUserIdByListingIdAsync(listingQueueEntry.ListingId);
			IdentityHelper.AuthorizeCurrentUserOrAdmin(sellerId, _currentUserContext);

			IEnumerable<ListingQueuePriorityEntry> listingQueuePriorityEntries = await _listingFetcher.GetListingQueuePriorityEntiresAsync(listingQueueEntry.ListingId);
			_priorityQueueDomainService.ValidatePriorityEntry(listingQueueEntry, listingQueuePriorityEntries, priorityWeight, shouldExist: false);
			ListingQueuePriorityEntry? listingQueuePriorityEntry = await _listingQueuePriorityEntryAdderService.AddListingQueuePriorityEntryAsync(listingQueueEntryId, priorityWeight);
			if (listingQueuePriorityEntry == null)
			{
				throw new InvalidOperationException("Can't create priority entry");
			}
			return listingQueuePriorityEntry;
		}
	}
}
