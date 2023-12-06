using Cifraex.Models;
using Microsoft.EntityFrameworkCore;

namespace Cifraex.Data
{
    public class ExchangeContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Account> Accounts { get; set; }


        public ExchangeContext(DbContextOptions<ExchangeContext> options)
            : base(options)
        {
        }
    }
}
