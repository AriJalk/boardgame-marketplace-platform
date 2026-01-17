using BoardGameMarket.UsersMicroservice.Core.Domain.RepositoryContracts;
using BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers;
using BoardGameMarket.UsersService.Core.Domain.Entities;
using BoardGameMarket.UsersService.Core.Domain.RepositoryContracts;

namespace BoardGameMarket.UsersService.Core.Application.Services.ApplicationUsers
{
	internal class UserDeleterService : IUserDeleterService
	{
		private readonly IUserRepository _repository;
		private readonly IUnitOfWork _unitOfWork;

		public UserDeleterService(IUserRepository usersRepository, IUnitOfWork unitOfWork)
		{
			_repository = usersRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task DeleteUserAsync(Guid targetUserId)
		{
			ApplicationUser? user = await _repository.GetUserByIdAsync(targetUserId);
			if (user == null)
			{
				throw new KeyNotFoundException("User");
			}
			 _repository.DeleteUserAsync(user);
			await _unitOfWork.SaveChangesAsync();
		}
	}
}
