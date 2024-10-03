namespace BankingSystem.DTO.Transactions
{
    public class TransferDTO
    {
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public double Amount { get; set; }
    }
}
