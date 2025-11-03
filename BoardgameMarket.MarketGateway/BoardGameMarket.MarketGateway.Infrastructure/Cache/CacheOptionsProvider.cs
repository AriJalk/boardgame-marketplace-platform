using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.MarketGateway.Infrastructure.Cache
{
	internal class CacheOptionsProvider : ICacheOptionsProvider
	{
		public DistributedCacheEntryOptions Default => new DistributedCacheEntryOptions()
			.SetAbsoluteExpiration(TimeSpan.FromSeconds(300))
			.SetSlidingExpiration(TimeSpan.FromSeconds(100));

		public DistributedCacheEntryOptions ShortLived => throw new NotImplementedException();

		public DistributedCacheEntryOptions LongLived => throw new NotImplementedException();
	}
}
