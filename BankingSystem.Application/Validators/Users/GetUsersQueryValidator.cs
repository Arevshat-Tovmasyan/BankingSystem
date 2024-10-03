using BankingSystem.Application.Queries.Users;
using FluentValidation;

namespace BankingSystem.Application.Validators.Users
{
    public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
    {
        public GetUsersQueryValidator()
        {
            RuleFor(x => x.Skip).InclusiveBetween(0, int.MaxValue);

            RuleFor(x => x.Take).InclusiveBetween(0, int.MaxValue);
        }
    }
}
