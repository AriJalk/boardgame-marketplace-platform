using BoardGameMarket.GamesService.Core.Domain.Entities;
using BoardGameMarket.GamesService.Core.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.GamesService.Core.Application.DTOs
{
	public class BggItemResponse
	{
		public int BggId { get; set; }
		public string? Name { get; set; }
		public IEnumerable<string> Categories { get; set; } = new List<string>();
	}
}
