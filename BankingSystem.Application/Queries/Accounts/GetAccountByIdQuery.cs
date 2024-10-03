using BankingSystem.Domain.Exceptions;
using BankingSystem.Domain.Interfaces;
using BankingSystem.DTO.Accounts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankingSystem.Application.Queries.Accounts
{
    public class GetAccountByIdQuery : IQuery<AccountDTO>
    {
        public Guid Id { get; set; }

        public GetAccountByIdQuery(Guid id)
        {
            Id = id;
        }
    }

    public class GetAccountByIdQueryHandler : BaseQueryHandler, IQueryHandler<GetAccountByIdQuery, AccountDTO>
    {
        public GetAccountByIdQueryHandler(IBankingDbContext bankingDbContext, IHttpContextAccessor contextAccessor, ILogger<GetAccountByIdQueryHandler> logger) : base(bankingDbContext, contextAccessor.HttpContext.User, logger)
        {
        }

        public async Task<AccountDTO> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var account = await _bankingDbContext.Accounts.AsNoTracking().Select(AccountDTO.GetSelector()).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (account == null)
            {
                throw new BankingException($"Account with the given id ({request.Id}) does not exist", ErrorCode.NotFound);
            }

            return account;
        }
    }
}
