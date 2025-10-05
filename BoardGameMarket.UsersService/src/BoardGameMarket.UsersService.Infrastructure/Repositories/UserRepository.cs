using BoardGameMarket.UsersMicroservice.Core.Domain.RepositoryContracts;
using BoardGameMarket.UsersService.Core.Domain.Entities;
using BoardGameMarket.UsersService.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.ApplicationsWithAppId;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.UsersService.Infrastructure.Repositories
{
	internal class UserRepository : IUserRepository
	{
		private readonly ApplicationDbContext _dbContext;

		public UserRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<ApplicationUser?> AddUserAsync(ApplicationUser user)
		{
			await _dbContext.Users.AddAsync(user);
			if (await _dbContext.SaveChangesAsync() > 0)
			{
				return user;
			}
			return null;
		}

		public async Task<bool> DeleteUserAsync(Guid userId)
		{
			ApplicationUser? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
			if (user != null)
			{
				_dbContext.Users.Remove(user);
				if (await _dbContext.SaveChangesAsync() > 0)
				{
					return true;
				}
			}
			return false;
		}

		public Task<ApplicationUser?> GetUserByIdAsync(Guid userId)
		{
			return _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
		}

		public async Task<IEnumerable<ApplicationUser>> GetUsersByIdsAsync(IEnumerable<Guid> userIds)
		{
			return await _dbContext.Users.Where(u => userIds.Contains(u.UserId)).ToListAsync();
		}

		public async Task<ApplicationUser?> UpdateUserAsync(ApplicationUser user)
		{
			ApplicationUser? userToUpdate = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);
			if (userToUpdate != null)
			{
				userToUpdate.FirstName = user.FirstName;
				userToUpdate.LastName = user.LastName;
				userToUpdate.Role = user.Role;
				userToUpdate.DisplayName = user.DisplayName;

				if (await _dbContext.SaveChangesAsync() > 0)
				{
					return userToUpdate;
				}
			}
			return null;
		}
	}
}
