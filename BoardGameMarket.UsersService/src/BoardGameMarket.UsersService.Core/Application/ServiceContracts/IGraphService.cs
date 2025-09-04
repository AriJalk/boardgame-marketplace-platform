using BoardGameMarket.UsersMicroservice.Core.Domain.Entities;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.UsersService.Core.Application.ServiceContracts
{
	public interface IGraphService
	{
		Task<User?> GetUserByUserIdAsync(Guid userId);
	}
}
