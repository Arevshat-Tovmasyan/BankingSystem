using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Exceptions;

namespace BankingSystem.Domain.Rules.Users
{
    public class UserAccountsShouldBeEmptyForDelete : IRule
    {
        public readonly List<Account> _accounts;

        public UserAccountsShouldBeEmptyForDelete(List<Account> accounts)
        {
            _accounts = accounts;
        }

        public string Message => "User accounts must be empty to be deleted";

        public ErrorCode ErrorCode => ErrorCode.Validation;

        public bool IsBroken()
        {
            return _accounts.Exists(x => x.Balance != 0);
        }
    }
}
