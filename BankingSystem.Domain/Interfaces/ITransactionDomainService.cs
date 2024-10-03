using BankingSystem.Domain.Entities;

namespace BankingSystem.Domain.Interfaces
{
    public interface ITransactionDomainService
    {
        Task<Transaction> DepositAsync(Guid accountId, double amount, CancellationToken cancellationToken);
        Task<Transaction> WithdrawAsync(Guid accountId, double amount, CancellationToken cancellationToken);
        Task<List<Transaction>> TransferAsync(Guid fromAccountId, Guid toAccountId, double amount, CancellationToken cancellationToken);

    }
}
