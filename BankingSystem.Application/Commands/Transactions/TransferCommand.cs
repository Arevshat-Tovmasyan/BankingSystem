using BankingSystem.Domain.Interfaces;
using BankingSystem.DTO.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankingSystem.Application.Commands.Transactions
{
    public class TransferCommand : ICommand<IReadOnlyList<TransactionDTO>>
    {
        public TransferDTO Transfer { get; set; }

        public TransferCommand(TransferDTO transfer)
        {
            Transfer = transfer;
        }
    }

    public class TransferCommandHandler : BaseCommandHandler, ICommandHandler<TransferCommand, IReadOnlyList<TransactionDTO>>
    {
        private readonly ITransactionDomainService _transactionDomainService;

        public TransferCommandHandler(IBankingDbContext bankingDbContext, IHttpContextAccessor contextAccessor, ILogger<TransferCommandHandler> logger, ITransactionDomainService transactionDomainService)
            : base(bankingDbContext, contextAccessor.HttpContext.User, logger)
        {
            _transactionDomainService = transactionDomainService;
        }

        public async Task<IReadOnlyList<TransactionDTO>> Handle(TransferCommand request, CancellationToken cancellationToken)
        {
            var transactions = await _transactionDomainService.TransferAsync(request.Transfer.FromAccountId, request.Transfer.ToAccountId, request.Transfer.Amount, cancellationToken);

            await _bankingDbContext.SaveChangesAsync(cancellationToken);

            var ids = transactions.Select(x => x.Id).ToHashSet();

            var result = await _bankingDbContext.Transactions.AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .Select(TransactionDTO.GetSelector())
                .ToArrayAsync(cancellationToken);

            return result;
        }
    }
}
