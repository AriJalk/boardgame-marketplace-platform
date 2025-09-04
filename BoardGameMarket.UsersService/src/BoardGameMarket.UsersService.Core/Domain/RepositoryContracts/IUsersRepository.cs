using BoardGameMarket.UsersMicroservice.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.UsersMicroservice.Core.Domain.RepositoryContracts
{
	public interface IUsersRepository
	{
		Task<ApplicationUser?> GetUserById(Guid userId);
		Task<ApplicationUser?> AddUser(ApplicationUser user);
		Task<ApplicationUser?> UpdateUser(ApplicationUser user);
		Task<bool> DeleteUser(Guid userId);
	}
}
