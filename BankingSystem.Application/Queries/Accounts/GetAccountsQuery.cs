using BankingSystem.Application.Behaviours;
using BankingSystem.Domain.Interfaces;
using BankingSystem.DTO.Accounts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankingSystem.Application.Queries.Accounts
{
    public class GetAccountsQuery : IValidationBehaviour, IQuery<IReadOnlyList<AccountDTO>>
    {
        public Guid? UserId { get; set; }
        public bool? IsClosed { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }

        public GetAccountsQuery(Guid? userId, bool? isClosed, int? skip, int? take)
        {
            UserId = userId;
            IsClosed = isClosed;
            Skip = skip;
            Take = take;
        }
    }

    public class GetAccountsQueryHandler : BaseQueryHandler, IQueryHandler<GetAccountsQuery, IReadOnlyList<AccountDTO>>
    {
        public GetAccountsQueryHandler(IBankingDbContext bankingDbContext, IHttpContextAccessor contextAccessor, ILogger<GetAccountsQueryHandler> logger) : base(bankingDbContext, contextAccessor.HttpContext.User, logger)
        {
        }

        public async Task<IReadOnlyList<AccountDTO>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        {
            var accountsQuery = _bankingDbContext.Accounts.AsNoTracking()
                                    .Select(AccountDTO.GetSelector());

            if (request.UserId.HasValue)
            {
                accountsQuery = accountsQuery.Where(x => x.UserId == request.UserId.Value);
            }
            
            if (request.IsClosed.HasValue)
            {
                accountsQuery = accountsQuery.Where(x => x.IsClosed == request.IsClosed.Value);
            }

            if (request.Skip.HasValue)
            {
                accountsQuery = accountsQuery.Skip(request.Skip.Value);
            }

            if (request.Take.HasValue)
            {
                accountsQuery = accountsQuery.Take(request.Take.Value);
            }

            var accounts = await accountsQuery.ToArrayAsync(cancellationToken);

            return accounts;
        }
    }
}
