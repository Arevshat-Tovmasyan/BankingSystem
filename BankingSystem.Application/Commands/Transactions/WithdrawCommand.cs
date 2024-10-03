using BankingSystem.Domain.Interfaces;
using BankingSystem.DTO.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankingSystem.Application.Commands.Transactions
{
    public class WithdrawCommand : ICommand<TransactionDTO>
    {
        public WithdrawDTO Withdraw { get; set; }

        public WithdrawCommand(WithdrawDTO withdraw)
        {
            Withdraw = withdraw;
        }
    }

    public class WithdrawCommandHandler : BaseCommandHandler, ICommandHandler<WithdrawCommand, TransactionDTO>
    {
        private readonly ITransactionDomainService _transactionDomainService;

        public WithdrawCommandHandler(IBankingDbContext bankingDbContext, IHttpContextAccessor contextAccessor, ILogger<WithdrawCommandHandler> logger, ITransactionDomainService transactionDomainService)
            : base(bankingDbContext, contextAccessor.HttpContext.User, logger)
        {
            _transactionDomainService = transactionDomainService;
        }

        public async Task<TransactionDTO> Handle(WithdrawCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionDomainService.WithdrawAsync(request.Withdraw.AccountId, request.Withdraw.Amount, cancellationToken);

            await _bankingDbContext.SaveChangesAsync(cancellationToken);

            var result = await _bankingDbContext.Transactions.AsNoTracking()
                .Select(TransactionDTO.GetSelector())
                .FirstAsync(x => x.Id == transaction.Id, cancellationToken);

            return result;
        }
    }
}
