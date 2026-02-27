using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingService.ServiceUnitTests
{
	internal class EntityTestHelper
	{
		public static Listing Clone(Listing source)
		{
			return new Listing()
			{
				Id = source.Id,
				CreatedAt = source.CreatedAt,
				Details = source.Details,
				ListingGroupId = source.ListingGroupId,
				ModifiedAt = source.ModifiedAt,
				Status = source.Status,
				Price = source.Price,
				SellerUserId = source.SellerUserId,
				Title = source.Title,
			};
		}
	}
}