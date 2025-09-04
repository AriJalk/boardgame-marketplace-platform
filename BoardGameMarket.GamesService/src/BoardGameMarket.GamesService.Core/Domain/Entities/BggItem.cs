using BoardGameMarket.GamesService.Core.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.GamesService.Core.Domain.Entities
{
	public class BggItem
	{
		public Guid Id { get; set; }
		public BggMetadata BggMetadata { get; set; } = new BggMetadata();
		public DateTime ModifiedTimestamp { get; set; }
	}
}
