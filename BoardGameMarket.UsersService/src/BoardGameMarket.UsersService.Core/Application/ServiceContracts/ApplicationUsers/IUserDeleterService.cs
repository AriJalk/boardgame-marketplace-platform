namespace BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers
{
	public interface IUserDeleterService
	{
		Task<bool> DeleteUserAsync(Guid targetUserId, Guid requesterId);
	}
}
