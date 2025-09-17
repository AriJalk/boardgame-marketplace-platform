using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.MarketOrchestrator.BusinessLogic.DTOs.ServiceEntities
{
	public record User(Guid UserId, IEnumerable<string> Roles, string? FirstName, string? LastName, string? DisplayName) { };
}
