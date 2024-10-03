using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Interfaces;
using BankingSystem.Domain.Rules.GenericRules;
using BankingSystem.Domain.Rules.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankingSystem.Domain.Services
{
    public class TransactionDomainService : BaseDomainService, ITransactionDomainService
    {
        public TransactionDomainService(IBankingDbContext bankingDbContext, ILogger<TransactionDomainService> logger) : base(bankingDbContext, logger)
        {
        }

        public async Task<Transaction> DepositAsync(Guid accountId, double amount, CancellationToken cancellationToken)
        {
            await CheckRuleAsync(new EntityExists<Account>(accountId, _bankingDbContext, cancellationToken));

            var existingAccount = await _bankingDbContext.Accounts.FirstAsync(x => x.Id == accountId, cancellationToken);

            CheckRule(new AccountShouldNotBeClosedForTransaction(existingAccount.IsClosed));

            existingAccount.DepositToAccount(amount);

            _bankingDbContext.Accounts.Update(existingAccount);

            var transaction = new Transaction(Guid.NewGuid(), amount, TransactionType.Deposit, accountId);

            var result = await _bankingDbContext.Transactions.AddAsync(transaction, cancellationToken);

            return result.Entity;
        }

        public async Task<List<Transaction>> TransferAsync(Guid fromAccountId, Guid toAccountId, double amount, CancellationToken cancellationToken)
        {
            await CheckRuleAsync(new EntityExists<Account>(fromAccountId, _bankingDbContext, cancellationToken));
            await CheckRuleAsync(new EntityExists<Account>(toAccountId, _bankingDbContext, cancellationToken));

            var fromAccount = await _bankingDbContext.Accounts.FirstAsync(x => x.Id == fromAccountId, cancellationToken);
            var toAccount = await _bankingDbContext.Accounts.FirstAsync(x => x.Id == toAccountId, cancellationToken);

            CheckRule(new AccountShouldNotBeClosedForTransaction(fromAccount.IsClosed));
            CheckRule(new AccountShouldNotBeClosedForTransaction(toAccount.IsClosed));

            CheckRule(new AmountRequiredForWithdrawalExists(fromAccount.Balance, amount));

            fromAccount.WithdrawFromAccount(amount);
            toAccount.DepositToAccount(amount);

            _bankingDbContext.Accounts.Update(fromAccount);
            _bankingDbContext.Accounts.Update(toAccount);

            var firstTransaction = new Transaction(Guid.NewGuid(), amount, TransactionType.Transfer, fromAccountId);
            var secondTransaction = new Transaction(Guid.NewGuid(), amount, TransactionType.Transfer, toAccountId);

            var firstResult = await _bankingDbContext.Transactions.AddAsync(firstTransaction, cancellationToken);
            var secondResult = await _bankingDbContext.Transactions.AddAsync(secondTransaction, cancellationToken);

            return new List<Transaction> { firstResult.Entity, secondResult.Entity };
        }

        public async Task<Transaction> WithdrawAsync(Guid accountId, double amount, CancellationToken cancellationToken)
        {
            await CheckRuleAsync(new EntityExists<Account>(accountId, _bankingDbContext, cancellationToken));

            var existingAccount = await _bankingDbContext.Accounts.FirstAsync(x => x.Id == accountId, cancellationToken);

            CheckRule(new AccountShouldNotBeClosedForTransaction(existingAccount.IsClosed));
            CheckRule(new AmountRequiredForWithdrawalExists(existingAccount.Balance, amount));

            existingAccount.WithdrawFromAccount(amount);

            _bankingDbContext.Accounts.Update(existingAccount);

            var transaction = new Transaction(Guid.NewGuid(), amount, TransactionType.Withdraw, accountId);

            var result = await _bankingDbContext.Transactions.AddAsync(transaction, cancellationToken);

            return result.Entity;
        }
    }
}
