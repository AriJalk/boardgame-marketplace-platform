using BoardGameMarket.UsersMicroservice.Core.Domain.RepositoryContracts;
using BoardGameMarket.UsersService.Core.Domain.Entities;
using BoardGameMarket.UsersService.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
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

		public async Task AddUserAsync(ApplicationUser user)
		{
			await _dbContext.Users.AddAsync(user);

		}

		public void DeleteUserAsync(ApplicationUser user)
		{
			_dbContext.Users.Remove(user);
		}

		public Task<ApplicationUser?> GetUserByIdAsync(Guid userId)
		{
			return _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
		}

		public async Task<IEnumerable<ApplicationUser>> GetUsersByIdsAsync(IEnumerable<Guid> userIds)
		{
			return await _dbContext.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
		}
	}
}
