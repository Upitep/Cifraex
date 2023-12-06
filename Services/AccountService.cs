using Cifraex.Data;
using Cifraex.Models;
using Microsoft.EntityFrameworkCore;

namespace Cifraex.Services
{
    public class AccountService
    {
        private readonly ExchangeContext _context;

        public AccountService(ExchangeContext context)
        {
            _context = context;
        }

        public async Task<Account> GetAccountAsync(int userId, string currencyCode)
        {
            return await _context.Accounts
                .Where(a => a.UserId == userId && a.Currency.CurrencyCode == currencyCode)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAccountBalanceAsync(int userId, string currencyCode, decimal newBalance)
        {
            var account = await GetAccountAsync(userId, currencyCode);

            if (account == null)
            {
                return false;
            }

            account.Balance = newBalance;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}