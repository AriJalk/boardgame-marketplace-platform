using BoardGameMarket.UsersMicroservice.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.UsersService.Core.Application.ServiceContracts
{
	internal interface IUserUpdaterService
	{
		Task<ApplicationUser?> UpdateUserAsync(ApplicationUser updatedUser, Guid requesterId);
	}
}
