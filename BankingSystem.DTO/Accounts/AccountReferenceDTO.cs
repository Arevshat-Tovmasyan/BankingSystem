using BankingSystem.Domain.Entities;

namespace BankingSystem.DTO.Accounts
{
    public class AccountReferenceDTO
    {
        public Guid? Id { get; set; }
        public ulong AccountNumber { get; set; }
        public AccountType Type { get; set; }
    }
}
