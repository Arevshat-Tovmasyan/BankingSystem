using BankingSystem.Application.Behaviours;
using BankingSystem.Domain.Exceptions;
using BankingSystem.Domain.Interfaces;
using BankingSystem.DTO.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankingSystem.Application.Queries.Transactions
{
    public class GetTransactionsByAccountIdQuery : IValidationBehaviour, IQuery<IReadOnlyList<TransactionDTO>>
    {
        public Guid AccountId { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }

        public GetTransactionsByAccountIdQuery(Guid accountId, int? skip, int? take)
        {
            AccountId = accountId;
            Skip = skip;
            Take = take;
        }
    }

    public class GetTransactionsByAccountIdQueryHandler : BaseQueryHandler, IQueryHandler<GetTransactionsByAccountIdQuery, IReadOnlyList<TransactionDTO>>
    {
        public GetTransactionsByAccountIdQueryHandler(IBankingDbContext bankingDbContext, IHttpContextAccessor contextAccessor, ILogger<GetTransactionsByAccountIdQueryHandler> logger)
            : base(bankingDbContext, contextAccessor.HttpContext.User, logger)
        {
        }

        public async Task<IReadOnlyList<TransactionDTO>> Handle(GetTransactionsByAccountIdQuery request, CancellationToken cancellationToken)
        {
            if (!await _bankingDbContext.Accounts.AsNoTracking().AnyAsync(x => x.Id == request.AccountId, cancellationToken))
            {
                throw new BankingException($"Account with the given id ({request.AccountId}) does not exist", ErrorCode.NotFound);
            }

            var transactionsQuery = _bankingDbContext.Transactions.AsNoTracking()
                                        .Where(x => x.AccountId == request.AccountId)
                                        .Select(TransactionDTO.GetSelector());

            if (request.Skip.HasValue)
            {
                transactionsQuery = transactionsQuery.Skip(request.Skip.Value);
            }

            if (request.Take.HasValue)
            {
                transactionsQuery = transactionsQuery.Take(request.Take.Value);
            }

            var transactions = await transactionsQuery.ToArrayAsync(cancellationToken);

            return transactions;
        }
    }
}
