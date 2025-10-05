using BoardGameMarket.UsersService.Core.Application.DTOs;
using BoardGameMarket.UsersService.Core.Application.Mappers;
using BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers;
using BoardGameMarket.UsersService.Core.Domain.Entities;
using BoardGameMarket.UsersService.Core.Domain.Enums;
using BoardGameMarket.UsersService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.UsersService.Core.Application.UseCases
{
	public class UpdateUserDetailsUseCase
	{
		private readonly IUserGetterService _userGetterService;
		private readonly IUnitOfWork _unitOfWork;

		public UpdateUserDetailsUseCase(IUserGetterService userGetterService, IUnitOfWork unitOfWork)
		{
			_userGetterService = userGetterService;
			_unitOfWork = unitOfWork;
		}

		public async Task<UserDto> ExecuteAsync(Guid requesterId, Guid targetId, UserUpdateRequest updateRequest)
		{
			ApplicationUser? requester = await _userGetterService.GetUserByUserIdAsync(requesterId);
			ApplicationUser? userToUpdate;
			bool isSameUser = requesterId == targetId;
			if (requester == null)
			{
				throw new InvalidOperationException();
			}
			if (!isSameUser && !requester.HasAnyRole(RoleType.Moderator | RoleType.Admin))
			{
				throw new UnauthorizedAccessException();
			}

			userToUpdate = isSameUser ? requester : await _userGetterService.GetUserByUserIdAsync(targetId) ?? throw new InvalidOperationException("Target user not found");

			userToUpdate.FirstName = updateRequest.FirstName;
			userToUpdate.LastName = updateRequest.LastName;
			userToUpdate.DisplayName = updateRequest.DisplayName;

			await _unitOfWork.SaveChangesAsync();
			return userToUpdate.ToDto();
		}
	}
}
