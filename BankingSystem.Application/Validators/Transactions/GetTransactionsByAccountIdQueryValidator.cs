using BankingSystem.Application.Queries.Transactions;
using FluentValidation;

namespace BankingSystem.Application.Validators.Transactions
{
    public class GetTransactionsByAccountIdQueryValidator : AbstractValidator<GetTransactionsByAccountIdQuery>
    {
        public GetTransactionsByAccountIdQueryValidator()
        {
            RuleFor(x => x.Skip).InclusiveBetween(0, int.MaxValue);

            RuleFor(x => x.Take).InclusiveBetween(0, int.MaxValue);
        }
    }
}
