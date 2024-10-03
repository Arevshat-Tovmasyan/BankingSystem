namespace BankingSystem.DTO.Transactions
{
    public class DepositDTO
    {
        public Guid AccountId { get; set; }
        public double Amount { get; set; }
    }
}
