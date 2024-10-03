namespace BankingSystem.Domain.Rules
{
    public class BaseAsyncRule
    {
        protected readonly CancellationToken _cancellationToken;

        public BaseAsyncRule(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
        }
    }
}
