using Microsoft.EntityFrameworkCore;
using Models.WalletDetails;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using WalletService.Models;
using WalletService.Repositories.Interfaces;

namespace WalletService.Repositories
{
    public class WalletRepo : IWalletRepo
    {
        private WalletContext WalletService { get; set; }

        public WalletRepo(WalletContext walletService)
        {
            WalletService = walletService;
        }

        public async Task<WalletDetails> CreateWalletAsync()
        {
            // Create a new wallet
            var wallet = new WalletDetails { Id = Guid.NewGuid(), Balance = 0, TransactionCount = 1 };
            WalletService.Wallets.Add(wallet);
            await WalletService.SaveChangesAsync();
            return wallet;
        }

        public async Task<WalletDetails> GetWalletAsync(Guid id)
        {
            return await WalletService.Wallets.FindAsync(id);
        }

        public async Task AddFundsAsync(Guid id, decimal amount)
        {

            // Get the wallet by Id
            var wallet = await WalletService.Wallets.FindAsync(id);
            if (wallet == null) throw new InvalidOperationException("Wallet not found.");

            // Increment fund
            wallet.Balance += amount;
            await WalletService.SaveChangesAsync();
        }

        public async Task RemoveFundsAsync(string userId, Guid id, decimal amount)
        {
            var wallet = await WalletService.Wallets.FindAsync(id);
            if (wallet == null) throw new InvalidOperationException("Wallet not found.");

            if (wallet.Balance < amount) throw new InvalidOperationException("Insufficient funds.");

            // Prevent user from spending the same funds twice
            // Get last transaction
            var lastTransaction = await WalletService.Transactions
                                        .Where(t => t.UserId == userId && t.WalletId == id && t.Amount == amount)
                                        .OrderByDescending(t => t.Timestamp)
                                        .FirstOrDefaultAsync();


            // Check if the last transaction was recent enough to prevent duplicate removal
            if (lastTransaction != null)
            {
                // check transactions from the last 5min
                if ((DateTime.UtcNow - lastTransaction.Timestamp).TotalMinutes < 5)
                {
                    throw new InvalidOperationException("Duplicate removal detected. Please try again later.");
                }
            }

            wallet.Balance -= amount;
            wallet.TransactionCount++; // Increment transaction for concurrency control

            try
            {
                await WalletService.SaveChangesAsync();

                // Record the transaction
                var transaction = new Transaction
                {
                    UserId = userId,
                    WalletId = id,
                    Amount = amount,
                    Timestamp = DateTime.UtcNow
                };
                WalletService.Transactions.Add(transaction);
                await WalletService.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                throw new InvalidOperationException("Concurrent update detected. Please try again.");
            }
        }

    }
}
