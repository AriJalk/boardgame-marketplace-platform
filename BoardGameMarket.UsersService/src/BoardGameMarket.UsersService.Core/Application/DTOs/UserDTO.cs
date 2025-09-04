using BoardGameMarket.UsersMicroservice.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.UsersService.Core.Application.DTOs
{
	public record UserDTO(Guid UserId, UserRole Role, string? FirstName, string? LastName, string? DisplayName) { };
}
