using BoardGameMarket.UsersService.Core.Domain.Entities;
using BoardGameMarket.UsersService.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;


namespace BoardGameMarket.UsersService.Infrastructure.DbContexts
{
	internal class ApplicationDbContext : DbContext
	{
		public DbSet<ApplicationUser> Users { get; set; }

		public ApplicationDbContext(DbContextOptions options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<ApplicationUser>().HasKey(u => u.Id);
		}
	}
}
