using BoardGameMarket.UsersService.Core.Domain.Entities;

namespace BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers
{
	public interface IUserGetterService
	{
		Task<ApplicationUser?> GetUserByUserIdAsync(Guid userId);
	}
}
