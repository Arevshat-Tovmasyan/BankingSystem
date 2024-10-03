using BankingSystem.Domain.Entities;
using System.Linq.Expressions;

namespace BankingSystem.DTO.Users
{
    public class UserDTO
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }


        public static Expression<Func<User, UserDTO>> GetSelector()
        {
            return user => new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
            };
        }

        public static User ToEntity(UserDTO userDTO)
        {
            return new User(
                id: userDTO.Id ?? Guid.NewGuid(),
                name: userDTO.Name);
        }
    }
}
