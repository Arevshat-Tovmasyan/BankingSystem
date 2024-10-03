namespace BankingSystem.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public double Amount { get; private set; }
        public DateTime Timestamp { get; private set; }
        public TransactionType Type { get; private set; }

        public Guid AccountId { get; private set; }
        public virtual Account Account { get; private set; }

        protected Transaction()
        {
                
        }

        public Transaction(Guid id, double amount, TransactionType type, Guid accountId)
        {
            Id = id;
            Amount = amount;
            Timestamp = DateTime.UtcNow;
            Type = type;
            AccountId = accountId;
        }
    }

    public enum TransactionType
    {
        Deposit = 0,
        Withdraw = 1,
        Transfer = 2
    }
}
