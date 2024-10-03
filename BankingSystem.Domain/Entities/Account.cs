namespace BankingSystem.Domain.Entities
{
    public class Account : BaseEntity
    {
        public ulong AccountNumber { get; private set; }
        public AccountType Type { get; private set; }
        public double Balance { get; private set; }
        public bool IsClosed { get; private set; }
        public DateTime CreatedDate { get; private set; }

        public Guid UserId { get; private set; }
        public virtual User User { get; private set; }

        public virtual List<Transaction> Transactions { get; private set; }

        protected Account()
        {
            Transactions = new List<Transaction>();
        }

        public Account(Guid id, ulong accountNumber, AccountType type, double balance, Guid userId) : this()
        {
            Id = id;
            AccountNumber = accountNumber;
            Type = type;
            Balance = balance;
            IsClosed = false; // Newly created account is not closed
            CreatedDate = DateTime.UtcNow;
            UserId = userId;
        }

        public void CloseAccount() => IsClosed = true;
        public void DepositToAccount(double amount) => Balance += amount;
        public void WithdrawFromAccount(double amount) => Balance -= amount;
    }

    public enum AccountType
    {
        Savings = 0,
        Checking = 1
    }
}
