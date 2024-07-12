using log4net;
using Microsoft.AspNetCore.Mvc;
using Models.WalletDetails;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using WalletService.Repositories.Interfaces;

namespace WalletService.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Produces("application/json")]
    [Route("api/WalletManagement")]
    public class WalletController : ControllerBase
    {

        static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IWalletRepo _walletRepo;
        public WalletController(IWalletRepo walletRepo)
        {
            _walletRepo = walletRepo;
        }

        [HttpPost]
        public async Task<ActionResult<WalletDetails>> CreateWallet()
        {

            Log.Info("Request to create a new wallet.");
            try
            {
                var wallet = await _walletRepo.CreateWalletAsync();
                Log.Info($"Wallet created with Id: {wallet.Id}");
                return CreatedAtAction(nameof(GetWallet), new { id = wallet.Id }, new { wallet.Id });
            }
            catch (Exception ex)
            {
                Log.Error($"Error creating wallet. Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WalletDetails>> GetWallet(Guid id)
        {
            Log.Info($"Request to get wallet with Id: {id}");

            try
            {
                var wallet = await _walletRepo.GetWalletAsync(id);
                if (wallet == null)
                {
                    Log.Warn($"Wallet with Id: {id} not found.");
                    return NotFound();
                }
                Log.Info($"Retrieved wallet with Id: {id} and balance: {wallet.Balance}");
                return Ok(new { wallet.Id, wallet.Balance });
            }
            catch (Exception ex)
            {
                Log.Error($"Error retrieving wallet with ID: {id}. Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{id}/add")]
        public async Task<IActionResult> AddFunds(Guid id, [FromBody] decimal amount)
        {
            Log.Info($"Request to add {amount} to wallet with Id: {id}");

            try
            {
                if(amount <= 0)
                {
                    return StatusCode(501, "Amount cannot be less than €1.");
                }

                await _walletRepo.AddFundsAsync(id, amount);
                Log.Info($"Successfully added {amount} to wallet with Id: {id}");
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error($"Error adding {amount} to wallet with Id: {id}. Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{id}/consume")]
        public async Task<IActionResult> RemoveFunds(Guid id, [FromBody] decimal amount)
        {

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Get user ID from claims

            Log.Info($"Request to consume {amount} from wallet with Id: {id}");

            try
            {
                await _walletRepo.RemoveFundsAsync(userId, id, amount);
                Log.Info($"Successfully deduced {amount} from wallet with Id: {id}");
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                Log.Warn($"Insufficient funds in wallet with Id: {id}. Exception: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error($"Error removing {amount} from wallet with Id: {id}. Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}