using BankingSystem.Application.Queries.Accounts;
using FluentValidation;

namespace BankingSystem.Application.Validators.Accounts
{
    public class GetAccountsQueryValidator : AbstractValidator<GetAccountsQuery>
    {
        public GetAccountsQueryValidator()
        {
            RuleFor(x => x.Skip).InclusiveBetween(0, int.MaxValue);

            RuleFor(x => x.Take).InclusiveBetween(0, int.MaxValue);
        }
    }
}
