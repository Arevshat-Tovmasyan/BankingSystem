using BankingSystem.Domain.Exceptions;

namespace BankingSystem.Domain.Rules.Transactions
{
    public class AmountRequiredForWithdrawalExists : IRule
    {
        private readonly double _balance;
        private readonly double _amount;

        public AmountRequiredForWithdrawalExists(double balance, double amount)
        {
            _balance = balance;
            _amount = amount;
        }

        public string Message => "The amount required for withdrawal does not exist";

        public ErrorCode ErrorCode => ErrorCode.Conflict;

        public bool IsBroken()
        {
            return _balance < _amount;
        }
    }
}
