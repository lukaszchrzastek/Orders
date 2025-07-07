using Microsoft.EntityFrameworkCore;
using Orders.Infrastructure.Persistence.Entities;


namespace Orders.Infrastructure.Persistence
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
		public DbSet<EmailEntity> Emails => Set<EmailEntity>();
		public DbSet<EmailAttachmentEntity> Attachments => Set<EmailAttachmentEntity>();

		public DbSet<OrderEntity> Orders => Set<OrderEntity>();
		public DbSet<OrderLineEntity> OrderLines => Set<OrderLineEntity>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<EmailAttachmentEntity>()
				.HasOne(a => a.Email)
				.WithMany(e => e.Attachments)
				.HasForeignKey(a => a.EmailEntityId);

			modelBuilder.Entity<OrderLineEntity>()
				.HasOne(a => a.Order)
				.WithMany(e => e.OrderLines)
				.HasForeignKey(a => a.OrderEntityId);
		}
	}
}
