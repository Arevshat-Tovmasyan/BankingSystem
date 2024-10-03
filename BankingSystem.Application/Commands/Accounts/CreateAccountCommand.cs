using BankingSystem.Domain.Interfaces;
using BankingSystem.DTO.Accounts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankingSystem.Application.Commands.Accounts
{
    public class CreateAccountCommand : ICommand<AccountDTO>
    {
        public AccountDTO Account { get; set; }

        public CreateAccountCommand(AccountDTO account)
        {
            Account = account;
        }
    }

    public class CreateAccountCommandHandler : BaseCommandHandler, ICommandHandler<CreateAccountCommand, AccountDTO>
    {
        private readonly IAccountDomainService _accountDomainService;

        public CreateAccountCommandHandler(IBankingDbContext assetsContext, IHttpContextAccessor contextAccessor, ILogger<CreateAccountCommandHandler> logger, IAccountDomainService accountDomainService) 
            : base(assetsContext, contextAccessor.HttpContext.User, logger)
        {
            _accountDomainService = accountDomainService;
        }

        public async Task<AccountDTO> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = AccountDTO.ToEntity(request.Account);

            var createdAccount = await _accountDomainService.CreateAsync(account, cancellationToken);

            await _bankingDbContext.SaveChangesAsync(cancellationToken);

            var result = await _bankingDbContext.Accounts.AsNoTracking()
                .Select(AccountDTO.GetSelector())
                .FirstAsync(x => x.Id == createdAccount.Id, cancellationToken);

            return result;
        }
    }
}
