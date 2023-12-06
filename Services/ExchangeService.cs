using Cifraex.Data;

namespace Cifraex.Services
{
    public class ExchangeService
    {
        private readonly ExchangeContext _context;
        private readonly AccountService _accountService;

        public ExchangeService(ExchangeContext context, AccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }

        public async Task<bool> ExchangeCurrencyAsync(int userId, string fromCurrencyCode, string toCurrencyCode, decimal amount, decimal exchangeRate, decimal commissionRate)
        {
            // Получение счетов для обмена
            var fromAccount = await _accountService.GetAccountAsync(userId, fromCurrencyCode);
            var toAccount = await _accountService.GetAccountAsync(userId, toCurrencyCode);

            if (fromAccount == null || toAccount == null)
            {
                throw new InvalidOperationException("Один из счетов не найден.");
            }

            if (fromAccount.Balance < amount)
            {
                throw new InvalidOperationException("Недостаточно средств для обмена.");
            }

            // Произведение обмена
            var exchangedAmount = amount * exchangeRate;
            var commission = exchangedAmount * commissionRate;
            var finalAmount = exchangedAmount - commission;

            fromAccount.Balance -= amount;
            toAccount.Balance += finalAmount;

            _context.Accounts.Update(fromAccount);
            _context.Accounts.Update(toAccount);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
