using Microsoft.EntityFrameworkCore;

namespace TaskManagerAPI.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<UserItem> Users { get; set; }
        public DbSet<TransactionItem> Transactions { get; set; }
        public DbSet<LoginHistoryItem> LoginHistory { get; set; }
        public DbSet<CategoryItem> Categories { get; set; }
        public DbSet<CurrencyItem> Currencies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Specify precision and scale for the 'Amount' column
            modelBuilder.Entity<TransactionItem>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)"); // Specify precision and scale here
        }

    }
}
