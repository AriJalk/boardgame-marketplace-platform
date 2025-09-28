using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingsSold
{
	public interface IListingSoldDeleterService
	{
		Task<bool> DeleteListingSoldAsync(Guid listingSoldId);
	}
}
