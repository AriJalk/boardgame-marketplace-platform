using BoardGameMarket.GamesService.Core.Domain.Entities;
using BoardGameMarket.GamesService.Core.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.GamesService.Core.Application.ServiceContracts
{
	/// <summary>
	/// Creates game objects
	/// </summary>
	public interface IBggItemFactory
	{
		/// <summary>
		/// Create a BggItem based on supplied metadata
		/// </summary>
		/// <param name="metadata">Corresponding game metadata</param>
		/// <returns>A constructed item object with supplied metadata</returns>
		BggItem CreateFromMetadata(BggMetadata metadata);
	}
}
