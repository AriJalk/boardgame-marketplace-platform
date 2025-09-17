using BoardGameMarket.BggCatalogService.Core.Domain.ValueObjects;

namespace BoardGameMarket.BggCatalogService.Core.Application.AdapterContracts
{
	/// <summary>
	/// Interface for BGG adapters
	/// </summary>
	public interface IBggClient
	{
		/// <summary>
		/// Returns metadata from BGG for a collection of id's
		/// </summary>
		/// <param name="bggIds">Collection of item id's</param>
		/// <returns>Collection of metadata of found id's</returns>
		Task<IEnumerable<RawBggMetadata>?> GetMultiBggMetadataAsync(IEnumerable<int> bggIds);
	}
}
