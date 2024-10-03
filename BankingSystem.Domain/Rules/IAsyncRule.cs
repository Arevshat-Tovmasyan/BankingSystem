using BankingSystem.Domain.Exceptions;

namespace BankingSystem.Domain.Rules
{
    public interface IAsyncRule
    {
        Task<bool> IsBroken();
        string Message { get; }
        ErrorCode ErrorCode { get; }
    }
}
