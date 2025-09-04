using BoardGameMarket.UsersMicroservice.Core.Domain.Entities;

namespace BoardGameMarket.UsersMicroservice.Core.Application.ServiceContracts
{
	public interface IUserGetterService
	{
		Task<ApplicationUser?> GetUserByUserIdAsync(Guid userId);
	}
}
