using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Exceptions;
using BankingSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Domain.Rules.GenericRules
{
    public class EntityIdMustBeUnique<T> : BaseAsyncRule, IAsyncRule where T : BaseEntity
    {
        private readonly Guid _id;
        private readonly IBankingDbContext _bankingDbContext;

        public EntityIdMustBeUnique(Guid id, IBankingDbContext bankingDbContext, CancellationToken cancellationToken) : base(cancellationToken)
        {
            _id = id;
            _bankingDbContext = bankingDbContext;
        }

        public string Message => $"{typeof(T).Name} with the given id ({_id}) already exists";

        public ErrorCode ErrorCode => ErrorCode.Conflict;

        public async Task<bool> IsBroken()
        {
            return await _bankingDbContext.Set<T>().AnyAsync(x => x.Id == _id, _cancellationToken);
        }
    }
}
