using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.Services.Helpers;
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

namespace BoardGameMarket.ListingsService.Core.Application.Services.Listings
{
	internal class ListingGetterService : IListingGetterService
	{
		private readonly IListingRepository _listingRepository;
		private readonly RequestCache _requestCache;
		private readonly ILogger<ListingGetterService> _logger;

		public ListingGetterService(IListingRepository listingRepository, RequestCache requestCache, ILogger<ListingGetterService> logger)
		{
			_listingRepository = listingRepository;
			_requestCache = requestCache;
			_logger = logger;
		}

		public async Task<Listing?> GetListingByIdAsync(Guid listingId)
		{
			_logger.LogInformation($"{nameof(ListingGetterService)}:{nameof(GetListingByIdAsync)}- get listing {listingId}");
			if (_requestCache.TryGetListingById(listingId, out var listing))
			{
				return listing;
			}
			listing = await _listingRepository.GetListingByIdAsync(listingId);
			if (listing != null)
			{
				_requestCache.AddListing(listing);
			}
			return listing;
		}

		public async Task<IEnumerable<Listing>> GetListingsByListingGroupIdAsync(Guid groupId)
		{
			_logger.LogInformation($"{nameof(ListingGetterService)}:{nameof(GetListingsByListingGroupIdAsync)}- get listing by group {groupId}");

			IReadOnlyList<Listing> listingsFromCache = _requestCache.GetListingsByGroupId(groupId);
			if (listingsFromCache.Count > 0)
			{
				return listingsFromCache;
			}


			IEnumerable<Listing> listingsFromDb = await _listingRepository.GetListingsByGroupIdAsync(groupId);
			if (listingsFromDb.Any())
			{
				_requestCache.AddListingsByGroupId(groupId, listingsFromDb);
			}
			return listingsFromDb;
		}

		public async Task<IEnumerable<Listing>> GetListingsByListingIds(IEnumerable<Guid> listingIds)
		{
			_logger.LogInformation($"{nameof(ListingGetterService)}:{nameof(GetListingsByListingIds)}- get listings by ids {listingIds}");

			List<Listing> listings = new List<Listing>();
			HashSet<Guid> missingIds = new HashSet<Guid>();
			foreach (Guid listingId in listingIds)
			{
				if (_requestCache.TryGetListingById(listingId, out var listing))
				{
					listings.Add(listing!);
				}
				else
				{
					missingIds.Add(listingId);
				}
			}
			IEnumerable<Listing> listingsFromDb = await _listingRepository.GetListingsByListingIdsAsync(missingIds);
			listings.AddRange(listingsFromDb);
			foreach (Listing listing in listingsFromDb)
			{
				_requestCache.AddListing(listing);
			}
			return listings;
		}
	}
}
