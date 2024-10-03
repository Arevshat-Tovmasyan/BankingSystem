using BankingSystem.Domain.Interfaces;
using BankingSystem.DTO.Accounts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankingSystem.Application.Commands.Accounts
{
    public class CloseAccountCommand : ICommand<AccountDTO>
    {
        public Guid Id { get; set; }

        public CloseAccountCommand(Guid id)
        {
            Id = id;
        }
    }

    public class CloseAccountCommandHandler : BaseCommandHandler, ICommandHandler<CloseAccountCommand, AccountDTO>
    {
        private readonly IAccountDomainService _accountDomainService;

        public CloseAccountCommandHandler(IBankingDbContext bankingDbContext, IHttpContextAccessor contextAccessor, ILogger<CloseAccountCommandHandler> logger, IAccountDomainService accountDomainService)
            : base(bankingDbContext, contextAccessor.HttpContext.User, logger)
        {
            _accountDomainService = accountDomainService;
        }

        public async Task<AccountDTO> Handle(CloseAccountCommand request, CancellationToken cancellationToken)
        {
            var closedAccount = await _accountDomainService.CloseAsync(request.Id, cancellationToken);

            await _bankingDbContext.SaveChangesAsync(cancellationToken);

            var result = await _bankingDbContext.Accounts.AsNoTracking()
                .Select(AccountDTO.GetSelector())
                .FirstAsync(x => x.Id == closedAccount.Id, cancellationToken);

            return result;
        }
    }
}
