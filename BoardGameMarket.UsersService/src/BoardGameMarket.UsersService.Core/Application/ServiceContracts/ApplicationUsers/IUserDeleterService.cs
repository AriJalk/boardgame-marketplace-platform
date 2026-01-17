using BoardGameMarket.UsersService.Core.Domain.Entities;

namespace BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers
{
	public interface IUserDeleterService
	{
		Task DeleteUserAsync(Guid targetUserId);
	}
}
