using BankingSystem.Domain.Interfaces;
using BankingSystem.DTO.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankingSystem.Application.Commands.Transactions
{
    public class DepositCommand : ICommand<TransactionDTO>
    {
        public DepositDTO Deposit { get; set; }

        public DepositCommand(DepositDTO deposit)
        {
            Deposit = deposit;
        }
    }

    public class DepositCommandHandler : BaseCommandHandler, ICommandHandler<DepositCommand, TransactionDTO>
    {
        private readonly ITransactionDomainService _transactionDomainService;

        public DepositCommandHandler(IBankingDbContext bankingDbContext, IHttpContextAccessor contextAccessor, ILogger<DepositCommandHandler> logger, ITransactionDomainService transactionDomainService)
            : base(bankingDbContext, contextAccessor.HttpContext.User, logger)
        {
            _transactionDomainService = transactionDomainService;
        }

        public async Task<TransactionDTO> Handle(DepositCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionDomainService.DepositAsync(request.Deposit.AccountId, request.Deposit.Amount, cancellationToken);

            await _bankingDbContext.SaveChangesAsync(cancellationToken);

            var result = await _bankingDbContext.Transactions.AsNoTracking()
                .Select(TransactionDTO.GetSelector())
                .FirstAsync(x => x.Id == transaction.Id, cancellationToken);

            return result;
        }
    }
}
