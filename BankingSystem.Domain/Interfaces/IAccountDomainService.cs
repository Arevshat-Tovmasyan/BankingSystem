using BankingSystem.Domain.Entities;

namespace BankingSystem.Domain.Interfaces
{
    public interface IAccountDomainService
    {
        Task<Account> CreateAsync(Account account, CancellationToken cancellationToken);
        Task<Account> CloseAsync(Guid id, CancellationToken cancellationToken);
    }
}
