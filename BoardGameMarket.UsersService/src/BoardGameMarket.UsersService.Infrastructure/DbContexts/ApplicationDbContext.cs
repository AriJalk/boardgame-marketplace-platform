using BoardGameMarket.UsersService.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace BoardGameMarket.UsersService.Infrastructure.DbContexts
{
	internal class ApplicationDbContext : DbContext
	{
		public DbSet<ApplicationUser> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<ApplicationUser>().HasKey(u => u.UserId);
		}
	}
}
