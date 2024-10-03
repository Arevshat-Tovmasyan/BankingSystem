using BankingSystem.Domain.Entities;
using System.Linq.Expressions;

namespace BankingSystem.DTO.Accounts
{
    public class AccountDTO
    {
        public Guid? Id { get; set; }
        public ulong AccountNumber { get; set; }
        public AccountType Type { get; set; }
        public double Balance { get; set; }
        public bool IsClosed { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UserId { get; set; }

        public static Expression<Func<Account, AccountDTO>> GetSelector()
        {
            return account => new AccountDTO
            {
                Id = account.Id,
                AccountNumber = account.AccountNumber,
                Type = account.Type,
                Balance = account.Balance,
                IsClosed = account.IsClosed,
                CreatedDate = account.CreatedDate,
                UserId = account.UserId,
            };
        }

        public static Account ToEntity(AccountDTO accountDTO)
        {
            return new Account(
                id: accountDTO.Id ?? Guid.NewGuid(),
                accountNumber: accountDTO.AccountNumber,
                type: accountDTO.Type,
                balance: accountDTO.Balance,
                userId: accountDTO.UserId);
        }
    }
}
