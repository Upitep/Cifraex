using Cifraex.Data;
using Cifraex.Models;
using Microsoft.EntityFrameworkCore;

namespace Cifraex.Services
{
    public class CurrencyService
    {
        private readonly ExchangeContext _context;

        public CurrencyService(ExchangeContext context)
        {
            _context = context;
        }

        public async Task<List<Currency>> GetAllCurrenciesAsync()
        {
            return await _context.Currencies.ToListAsync();
        }

        public async Task<Currency> GetCurrencyByCodeAsync(string currencyCode)
        {
            return await _context.Currencies
                .FirstOrDefaultAsync(c => c.CurrencyCode == currencyCode);
        }

        public async Task<Currency> CreateCurrencyAsync(Currency currency)
        {
            _context.Currencies.Add(currency);
            await _context.SaveChangesAsync();
            return currency;
        }

        public async Task<bool> DeleteCurrencyAsync(string currencyCode)
        {
            var currency = await _context.Currencies
                .FirstOrDefaultAsync(c => c.CurrencyCode == currencyCode);

            if (currency == null)
            {
                return false;
            }

            // Удаляем все счета, связанные с этой валютой
            var accounts = await _context.Accounts
                .Where(a => a.Currency.CurrencyCode == currencyCode)
                .ToListAsync();

            if (accounts.Any())
            {
                _context.Accounts.RemoveRange(accounts);
            }

            _context.Currencies.Remove(currency);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateCurrencyAsync(string currencyCode, Currency updatedCurrency)
        {
            var currency = await _context.Currencies
                .FirstOrDefaultAsync(c => c.CurrencyCode == currencyCode);

            if (currency == null)
            {
                return false;
            }

            currency.Description = updatedCurrency.Description;

            _context.Entry(currency).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}