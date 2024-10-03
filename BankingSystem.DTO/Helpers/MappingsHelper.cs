using BankingSystem.Domain.Entities;
using BankingSystem.DTO.Accounts;

namespace BankingSystem.DTO.Helpers
{
    public static class MappingsHelper
    {
        public static List<AccountReferenceDTO> MapAccountToReferenceDTO(List<Account> accounts)
        {
            var result = new List<AccountReferenceDTO>();

            foreach (var account in accounts)
            {
                var accountReference = new AccountReferenceDTO() { Id = account.Id, AccountNumber = account.AccountNumber, Type = account.Type };

                result.Add(accountReference);
            }

            return result;
        }
    }
}
