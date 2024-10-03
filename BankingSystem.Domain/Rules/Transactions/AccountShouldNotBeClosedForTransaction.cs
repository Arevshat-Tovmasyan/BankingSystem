using BankingSystem.Domain.Exceptions;

namespace BankingSystem.Domain.Rules.Transactions
{
    public class AccountShouldNotBeClosedForTransaction : IRule
    {
        private readonly bool _isClosed;

        public AccountShouldNotBeClosedForTransaction(bool isClosed)
        {
            _isClosed = isClosed;
        }

        public string Message => "The account should not be closed for transactions";

        public ErrorCode ErrorCode => ErrorCode.Conflict;

        public bool IsBroken()
        {
            return _isClosed;
        }
    }
}
