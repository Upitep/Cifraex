using Cifraex.Data;
using Cifraex.Models;
using Microsoft.EntityFrameworkCore;

namespace Cifraex.Services
{
    public class UserService
    {
        private readonly ExchangeContext _context;
        private readonly AccountService _accountService;

        public UserService(ExchangeContext context, AccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Accounts)
                .ThenInclude(a => a.Currency)
                .ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Accounts)
                .ThenInclude(a => a.Currency)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            var allCurrencies = await _context.Currencies.ToListAsync();
            foreach (var currency in allCurrencies)
            {
                user.Accounts.Add(new Account { Currency = currency, Balance = 0 });
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UpdateUserAsync(int userId, User updatedUser)
        {
            var existingUser = await _context.Users.FindAsync(userId);
            if (existingUser == null)
            {
                return false;
            }

            _context.Entry(existingUser).CurrentValues.SetValues(updatedUser);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateUserBalanceAsync(int userId, string currency, decimal amount)
        {
            return await _accountService.UpdateAccountBalanceAsync(userId, currency, amount);
        }
    }
}
