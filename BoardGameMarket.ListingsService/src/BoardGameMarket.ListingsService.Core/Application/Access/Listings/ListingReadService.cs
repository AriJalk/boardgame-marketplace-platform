using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Access.Listings
{
	internal class ListingReadService : IListingReadService
	{
		private readonly IListingRepository _listingRepository;

		public ListingReadService(IListingRepository listingRepository)
		{
			_listingRepository = listingRepository;
		}

		public async Task<Listing?> GetListingByIdAsync(Guid listingId)
		{
			return await _listingRepository.GetListingByIdAsync(listingId);
		}

		public async Task<IEnumerable<Listing>> GetListingsByListingGroupIdAsync(Guid listingGroupId)
		{
			return await _listingRepository.GetListingsByGroupIdAsync(listingGroupId);
		}

		public async Task<IEnumerable<Listing>> GetListingsByIdsAsync(IEnumerable<Guid> listingIds)
		{
			return await _listingRepository.GetListingsByListingIdsAsync(listingIds);
		}
	}
}
