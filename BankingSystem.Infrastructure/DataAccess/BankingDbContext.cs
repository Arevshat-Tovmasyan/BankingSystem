using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Infrastructure.DataAccess
{
    public class BankingDbContext : DbContext, IBankingDbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }

        public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options)
        {

        }
    }
}
