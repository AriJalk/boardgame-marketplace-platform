using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingGroups
{
	public class ListingGroupDeleterService : IListingGroupDeleterService
	{
		private readonly IListingGroupRepository _listingGroupRepository;
		private readonly ILogger<ListingGroupDeleterService> _logger;

		public ListingGroupDeleterService(IListingGroupRepository listingGroupRepository, ILogger<ListingGroupDeleterService> logger)
		{
			_listingGroupRepository = listingGroupRepository;
			_logger = logger;
		}

		public async Task<bool> DeleteListingGroup(Guid listingGroupId)
		{
			_logger.LogInformation($"{nameof(ListingGroupDeleterService)}:{nameof(DeleteListingGroup)}- Deleting listing group {listingGroupId}");

			return await _listingGroupRepository.DeleteListingGroup(listingGroupId);
		}
	}
}
