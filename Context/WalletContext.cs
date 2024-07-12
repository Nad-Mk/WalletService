using Microsoft.EntityFrameworkCore;
using Models.WalletDetails;
using WalletService.Models;

public class WalletContext : DbContext
{
    public WalletContext(DbContextOptions<WalletContext> options) : base(options) { }

    public DbSet<WalletDetails> Wallets { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WalletDetails>()
            .Property(w => w.TransactionCount)
            .IsConcurrencyToken();
    }

}
