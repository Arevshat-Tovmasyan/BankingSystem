using BankingSystem.Domain.Entities;
using System.Linq.Expressions;

namespace BankingSystem.DTO.Transactions
{
    public class TransactionDTO
    {
        public Guid? Id { get; set; }
        public double Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public TransactionType Type { get; set; }
        public Guid AccountId { get; set; }

        public static Expression<Func<Transaction, TransactionDTO>> GetSelector()
        {
            return transaction => new TransactionDTO
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Type = transaction.Type,
                Timestamp = transaction.Timestamp,
                AccountId = transaction.AccountId,
            };
        }

        public static Transaction ToEntity(TransactionDTO transactionDTO)
        {
            return new Transaction(
                id: transactionDTO.Id ?? Guid.NewGuid(),
                amount: transactionDTO.Amount,
                type: transactionDTO.Type,
                accountId: transactionDTO.AccountId);
        }
    }
}
