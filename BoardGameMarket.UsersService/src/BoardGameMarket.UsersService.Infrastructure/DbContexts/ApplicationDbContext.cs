using BoardGameMarket.UsersService.Core.Domain.Entities;
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

			modelBuilder.Entity<ApplicationUser>(entity =>
			{
				entity.ToTable("users", "public");
				entity.HasKey(u => u.Id);
				entity.Property(u => u.Id).HasColumnName("id");
				entity.Property(u => u.FirstName).HasColumnName("first_name");
                entity.Property(u => u.LastName).HasColumnName("last_name");
                entity.Property(u => u.DisplayName).HasColumnName("display_name");
                entity.Property(u => u.Email).HasColumnName("email");
            });
		}
	}
}
