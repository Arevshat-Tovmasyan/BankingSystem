using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Interfaces;
using BankingSystem.Domain.Rules.GenericRules;
using BankingSystem.Domain.Rules.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankingSystem.Domain.Services
{
    public class UserDomainService : BaseDomainService, IUserDomainService
    {
        public UserDomainService(IBankingDbContext bankingDbContext, ILogger<UserDomainService> logger) : base(bankingDbContext, logger)
        {
        }

        public async Task<User> CreateAsync(User user, CancellationToken cancellationToken)
        {
            if (user.Id == Guid.Empty)
            {
                user.InitializeId();
            }
            else
            {
                await CheckRuleAsync(new EntityIdMustBeUnique<User>(user.Id, _bankingDbContext, cancellationToken));
            }

            var newUser = new User(user.Id, user.Name);
            var result = await _bankingDbContext.Users.AddAsync(newUser, cancellationToken);

            return result.Entity;
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await CheckRuleAsync(new EntityExists<User>(id, _bankingDbContext, cancellationToken));

            var existing = await _bankingDbContext.Users.Include(x => x.Accounts).FirstAsync(x => x.Id == id, cancellationToken);

            CheckRule(new UserAccountsShouldBeEmptyForDelete(existing.Accounts));

            _bankingDbContext.Users.Remove(existing);
        }

        public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            await CheckRuleAsync(new EntityExists<User>(user.Id, _bankingDbContext, cancellationToken));

            var existing = await _bankingDbContext.Users.FirstAsync(x => x.Id == user.Id, cancellationToken);

            existing.Update(user);

            var result = _bankingDbContext.Users.Update(existing);

            return result.Entity;
        }
    }
}
