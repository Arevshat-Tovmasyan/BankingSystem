using BankingSystem.Domain.Exceptions;
using BankingSystem.Domain.Interfaces;
using BankingSystem.DTO.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankingSystem.Application.Queries.Transactions
{
    public class GetTransactionByIdQuery : IQuery<TransactionDTO>
    {
        public Guid Id { get; set; }

        public GetTransactionByIdQuery(Guid id)
        {
            Id = id;
        }
    }

    public class GetTransactionByIdQueryHandler : BaseQueryHandler, IQueryHandler<GetTransactionByIdQuery, TransactionDTO>
    {
        public GetTransactionByIdQueryHandler(IBankingDbContext bankingDbContext, IHttpContextAccessor contextAccessor, ILogger<GetTransactionByIdQueryHandler> logger) : base(bankingDbContext, contextAccessor.HttpContext.User, logger)
        {
        }

        public async Task<TransactionDTO> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var transaction = await _bankingDbContext.Transactions.AsNoTracking().Select(TransactionDTO.GetSelector()).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (transaction == null)
            {
                throw new BankingException($"Transaction with the given id ({request.Id}) does not exist", ErrorCode.NotFound);
            }

            return transaction;
        }
    }
}
