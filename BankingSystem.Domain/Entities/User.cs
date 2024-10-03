namespace BankingSystem.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; private set; }
        public virtual List<Account> Accounts { get; private set; }

        protected User()
        {
            Accounts = new List<Account>();
        }

        public User(Guid id, string name) : this()
        {
            Id = id;
            Name = name;
        }

        public void Update(User user)
        {
            Name = user.Name;
        }
    }
}
