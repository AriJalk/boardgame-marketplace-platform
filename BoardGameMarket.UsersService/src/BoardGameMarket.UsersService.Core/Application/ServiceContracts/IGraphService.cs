using Microsoft.Graph.Models;

namespace BoardGameMarket.UsersService.Core.Application.ServiceContracts
{
	public interface IGraphService
	{
		Task<User?> GetUserByUserIdAsync(Guid userId);
	}
}
