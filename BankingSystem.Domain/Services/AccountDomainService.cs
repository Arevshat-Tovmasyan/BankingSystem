using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Interfaces;
using BankingSystem.Domain.Rules.Accounts;
using BankingSystem.Domain.Rules.GenericRules;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankingSystem.Domain.Services
{
    public class AccountDomainService : BaseDomainService, IAccountDomainService
    {
        public AccountDomainService(IBankingDbContext bankingDbContext, ILogger<AccountDomainService> logger) : base(bankingDbContext, logger)
        {
        }

        public async Task<Account> CreateAsync(Account account, CancellationToken cancellationToken)
        {
            if (account.Id == Guid.Empty)
            {
                account.InitializeId();
            }
            else
            {
                await CheckRuleAsync(new EntityIdMustBeUnique<Account>(account.Id, _bankingDbContext, cancellationToken));
            }

            await CheckRuleAsync(new EntityExists<User>(account.UserId, _bankingDbContext, cancellationToken));

            var newAccount = new Account(account.Id, account.AccountNumber, account.Type, account.Balance, account.UserId);

            var result = await _bankingDbContext.Accounts.AddAsync(newAccount, cancellationToken);

            return result.Entity;
        }

        public async Task<Account> CloseAsync(Guid id, CancellationToken cancellationToken)
        {
            await CheckRuleAsync(new EntityExists<Account>(id, _bankingDbContext, cancellationToken));

            var existing = await _bankingDbContext.Accounts.FirstAsync(x => x.Id == id, cancellationToken);

            CheckRule(new ClosedAccountBalanceMustBeEmpty(existing.Balance));

            existing.CloseAccount();

            var result = _bankingDbContext.Accounts.Update(existing);

            return result.Entity;
        }
    }
}
