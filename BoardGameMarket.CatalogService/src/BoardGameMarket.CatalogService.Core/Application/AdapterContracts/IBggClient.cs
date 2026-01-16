using BoardGameMarket.CatalogService.Core.Domain.ValueObjects;

namespace BoardGameMarket.CatalogService.Core.Application.AdapterContracts
{
	/// <summary>
	/// Interface for BGG adapters
	/// </summary>
	public interface IBggClient
	{
		/// <summary>
		/// Returns Metadata from BGG for a collection of id's
		/// </summary>
		/// <param name="bggIds">Collection of item id's</param>
		/// <returns>Collection of Metadata of found id's</returns>
		Task<IEnumerable<RawBggMetadata>> GetMultiBggMetadataAsync(IEnumerable<int> bggIds);
	}
}
