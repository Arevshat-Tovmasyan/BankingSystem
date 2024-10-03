namespace BankingSystem.Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; protected set; }

        public void InitializeId()
        {
            Id = Guid.NewGuid();
        }
    }
}
