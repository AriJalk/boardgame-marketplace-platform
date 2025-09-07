using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingGroups
{
	public class ListingGroupAdderService : IListingGroupAdderService
	{
		private readonly IListingGroupRepository _listingGroupRepository;
		private readonly ILogger<ListingGroupAdderService> _logger;

		public ListingGroupAdderService(IListingGroupRepository listingGroupRepository, ILogger<ListingGroupAdderService> logger)
		{
			_listingGroupRepository = listingGroupRepository;
			_logger = logger;
		}

		public async Task<ListingGroup?> AddListingGroup(ListingGroupAddRequest? listingGroupAddRequest)
		{
			ArgumentNullException.ThrowIfNull(listingGroupAddRequest, nameof(listingGroupAddRequest));

			_logger.LogInformation($"{nameof(ListingGroupAdderService)}:{nameof(AddListingGroup)}- Adding listing group");

			ListingGroup listingGroupToAdd = listingGroupAddRequest.ToListingGroup();

			listingGroupToAdd.Id = Guid.NewGuid();
			listingGroupToAdd.CreatedAt = DateTime.UtcNow;
			listingGroupToAdd.ModifiedAt = DateTime.UtcNow;

			return await _listingGroupRepository.AddListingGroupAsync(listingGroupToAdd);
		}
	}
}
