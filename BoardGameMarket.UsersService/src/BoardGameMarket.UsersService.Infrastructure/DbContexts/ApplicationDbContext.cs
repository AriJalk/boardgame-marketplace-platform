using BoardGameMarket.UsersService.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace BoardGameMarket.UsersService.Infrastructure.DbContexts
{
	internal class ApplicationDbContext : DbContext
	{
		public DbSet<ApplicationUser> Users { get; set; }
		public DbSet<ApplicationRole> Roles { get; set; }
		public DbSet<UserRole> UserRoles { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<ApplicationUser>().HasKey(u => u.UserId);
			modelBuilder.Entity<ApplicationRole>().HasKey(r => r.RoleId);
			modelBuilder.Entity<UserRole>().HasKey(ur => ur.UserRoleId);
			modelBuilder.Entity<UserRole>()
				.HasOne(ur => ur.User)
				.WithMany(u => u.UserRoles)
				.HasForeignKey(ur => ur.UserId);

			modelBuilder.Entity<UserRole>()
				.HasOne(ur => ur.Role)
				.WithMany(u => u.UserRoles)
				.HasForeignKey(ur => ur.RoleId);

		}
	}
}
