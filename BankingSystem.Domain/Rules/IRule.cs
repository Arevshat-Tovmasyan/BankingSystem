using BankingSystem.Domain.Exceptions;

namespace BankingSystem.Domain.Rules
{
    public interface IRule
    {
        bool IsBroken();
        string Message { get; }
        ErrorCode ErrorCode { get; }
    }
}
