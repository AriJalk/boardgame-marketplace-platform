using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.MarketGateway.Infrastructure.Cache
{
	internal interface ICacheOptionsProvider
	{
		DistributedCacheEntryOptions Default { get; }
		DistributedCacheEntryOptions ShortLived { get; }
		DistributedCacheEntryOptions LongLived { get; }
	}
}
