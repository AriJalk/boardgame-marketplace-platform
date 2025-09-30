using BoardGameMarket.UsersMicroservice.Core.Domain.RepositoryContracts;
using BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers;

namespace BoardGameMarket.UsersService.Core.Application.Services.ApplicationUsers
{
	internal class UserDeleterService : IUserDeleterService
	{
		private readonly IUserRepository _usersRepository;

		public UserDeleterService(IUserRepository usersRepository)
		{
			_usersRepository = usersRepository;
		}

		public async Task<bool> DeleteUserAsync( Guid targetUserId)
		{
			return await _usersRepository.DeleteUserAsync(targetUserId);
		}
	}
}
