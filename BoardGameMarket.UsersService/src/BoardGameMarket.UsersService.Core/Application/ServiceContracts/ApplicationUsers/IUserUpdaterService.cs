using BoardGameMarket.UsersService.Core.Domain.Entities;

namespace BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers
{
	public interface IUserUpdaterService
	{
		Task<ApplicationUser?> UpdateUserAsync(ApplicationUser updatedUser);
	}
}
