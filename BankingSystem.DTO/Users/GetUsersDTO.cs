using BankingSystem.Domain.Entities;
using BankingSystem.DTO.Accounts;
using BankingSystem.DTO.Helpers;
using System.Linq.Expressions;

namespace BankingSystem.DTO.Users
{
    public class GetUsersDTO
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public List<AccountReferenceDTO> Accounts { get; set; }


        public static Expression<Func<User, GetUsersDTO>> GetSelector()
        {
            return user => new GetUsersDTO
            {
                Id = user.Id,
                Name = user.Name,
                Accounts = MappingsHelper.MapAccountToReferenceDTO(user.Accounts),
            };
        }

        public static User ToEntity(GetUsersDTO userDTO)
        {
            return new User(
                id: userDTO.Id ?? Guid.NewGuid(),
                name: userDTO.Name);
        }
    }
}
