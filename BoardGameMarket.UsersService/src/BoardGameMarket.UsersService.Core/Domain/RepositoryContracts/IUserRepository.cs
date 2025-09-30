using BoardGameMarket.UsersService.Core.Domain.Entities;

namespace BoardGameMarket.UsersMicroservice.Core.Domain.RepositoryContracts
{
	public interface IUserRepository
	{
		Task<ApplicationUser?> GetUserByIdAsync(Guid userId);
		Task<IEnumerable<ApplicationUser>> GetUsersByIdsAsync(IEnumerable<Guid> userIds);
		Task<ApplicationUser?> AddUserAsync(ApplicationUser user);
		Task<bool> DeleteUserAsync(Guid userId);
		Task<ApplicationUser?> UpdateUserAsync(ApplicationUser user);
	}
}
