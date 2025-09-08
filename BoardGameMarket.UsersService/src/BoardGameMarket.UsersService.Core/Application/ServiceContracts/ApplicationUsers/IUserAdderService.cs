using BoardGameMarket.UsersService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers
{
	public interface IUserAdderService
	{
		Task<ApplicationUser?> AddUserAsync(ApplicationUser user);
	}
}
