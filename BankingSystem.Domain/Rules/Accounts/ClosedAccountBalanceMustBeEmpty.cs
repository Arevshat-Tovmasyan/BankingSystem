using BankingSystem.Domain.Exceptions;

namespace BankingSystem.Domain.Rules.Accounts
{
    public class ClosedAccountBalanceMustBeEmpty : IRule
    {
        private readonly double _balance;

        public ClosedAccountBalanceMustBeEmpty(double balance)
        {
            _balance = balance;
        }

        public string Message => "To close an account, the balance must be empty";

        public ErrorCode ErrorCode => ErrorCode.Conflict;

        public bool IsBroken()
        {
            return _balance != 0;
        }
    }
}
