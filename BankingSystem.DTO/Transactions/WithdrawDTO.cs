namespace BankingSystem.DTO.Transactions
{
    public class WithdrawDTO
    {
        public Guid AccountId { get; set; }
        public double Amount { get; set; }
    }
}
