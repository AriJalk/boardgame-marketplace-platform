using BoardGameMarket.UsersService.Core.Domain.Entities;

namespace BoardGameMarket.UsersMicroservice.Core.Domain.RepositoryContracts
{
	public interface IUserRepository
	{
		Task<ApplicationUser?> GetUserByIdAsync(Guid userId);
		Task<IEnumerable<ApplicationUser>> GetUsersByIdsAsync(IEnumerable<Guid> userIds);
		Task AddUserAsync(ApplicationUser user);
		void DeleteUserAsync(ApplicationUser user);
	}
}
