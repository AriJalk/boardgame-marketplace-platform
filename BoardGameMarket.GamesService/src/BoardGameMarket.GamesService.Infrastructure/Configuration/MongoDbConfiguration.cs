using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.GamesService.Infrastructure.Configuration
{
	internal class MongoDbConfiguration
	{
		public string? MongoHost { get; set; }
		public string? MongoPort { get; set; }
		public string? DatabaseName { get; set; }
		public string? CollectionName { get; set; }
	}
}
