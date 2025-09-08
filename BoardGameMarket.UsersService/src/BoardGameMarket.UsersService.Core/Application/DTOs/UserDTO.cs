using BoardGameMarket.UsersService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.UsersService.Core.Application.DTOs
{
	public record UserDTO(Guid UserId, IEnumerable<string> Roles, string? FirstName, string? LastName, string? DisplayName) { };
}
