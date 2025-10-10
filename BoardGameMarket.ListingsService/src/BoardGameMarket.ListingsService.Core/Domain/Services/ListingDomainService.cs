using BoardGameMarket.ListingsService.Core.Domain.Common;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Domain.Services
{
	internal class ListingDomainService
	{
		private Result ValidateStatus(Listing listing, ListingStatus requiredStatus)
		{
			List<string> errors = new List<string>();
			if (listing.Status != requiredStatus)
			{
				errors.Add($"Listing must be in {requiredStatus.ToString()} status");
			}
			if (errors.Count > 0)
			{
				return Result.Failure(errors);
			}
			return Result.Success();
		}

		public Result CanReserve(Listing listing)
		{
			return ValidateStatus(listing, ListingStatus.Active);
		}

		public Result CanSell(Listing listing)
		{
			return ValidateStatus(listing, ListingStatus.Reserved);
		}


	}
}
