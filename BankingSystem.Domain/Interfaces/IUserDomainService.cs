using BankingSystem.Domain.Entities;

namespace BankingSystem.Domain.Interfaces
{
    public interface IUserDomainService
    {
        Task<User> CreateAsync(User user, CancellationToken cancellationToken);
        Task<User> UpdateAsync(User user, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
