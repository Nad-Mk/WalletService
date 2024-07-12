using Models.WalletDetails;
using System; 
using System.Threading.Tasks;

namespace WalletService.Repositories.Interfaces
{
    public interface IWalletRepo
    {
        Task<WalletDetails> CreateWalletAsync();
        Task<WalletDetails> GetWalletAsync(Guid id);
        Task AddFundsAsync(Guid id, decimal amount);
        Task RemoveFundsAsync(string userId, Guid id, decimal amount);
    }
}
