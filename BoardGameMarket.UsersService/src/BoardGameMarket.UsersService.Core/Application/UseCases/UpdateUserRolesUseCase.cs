using BoardGameMarket.UsersService.Core.Application.DTOs;
using BoardGameMarket.UsersService.Core.Application.Mappers;
using BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers;
using BoardGameMarket.UsersService.Core.Domain.Entities;
using BoardGameMarket.UsersService.Core.Domain.Enums;
using BoardGameMarket.UsersService.Core.Domain.RepositoryContracts;
using BoardGameMarket.UsersService.Core.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.UsersService.Core.Application.UseCases
{
	public class UpdateUserRolesUseCase
	{
		private readonly IUserGetterService _userGetterService;
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserUpdateDomainService _userUpdateDomainService;

		public UpdateUserRolesUseCase(IUserGetterService userGetterService, IUnitOfWork unitOfWork, UserUpdateDomainService userUpdateDomainService)
		{
			_userGetterService = userGetterService;
			_unitOfWork = unitOfWork;
			_userUpdateDomainService = userUpdateDomainService;
		}

		public async Task<UserDto> ExecuteAsync(Guid requesterId, Guid targetId, RoleType targetRole)
		{
			if (requesterId == targetId)
			{
				throw new InvalidOperationException("Can't assign targetRole to self");
			}
			ApplicationUser? requester = await _userGetterService.GetUserByUserIdAsync(requesterId);
			if (requester == null)
			{
				throw new InvalidOperationException();
			}
			ApplicationUser? targetUser = await _userGetterService.GetUserByUserIdAsync(targetId);
			if (targetUser == null)
			{
				throw new InvalidOperationException();
			}


			_userUpdateDomainService.ValidateRoleAssignment(requester.Role, targetUser.Role, targetRole);

			targetUser.Role = targetRole;
			await _unitOfWork.SaveChangesAsync();
			return targetUser.ToDto();
		} 
	}
}
