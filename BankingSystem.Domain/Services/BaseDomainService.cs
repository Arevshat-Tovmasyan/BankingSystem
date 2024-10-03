using BankingSystem.Domain.Exceptions;
using BankingSystem.Domain.Interfaces;
using BankingSystem.Domain.Rules;
using Microsoft.Extensions.Logging;

namespace BankingSystem.Domain.Services
{
    public abstract class BaseDomainService
    {
        protected readonly ILogger _logger;
        public readonly IBankingDbContext _bankingDbContext;

        protected BaseDomainService(IBankingDbContext bankingDbContext, ILogger logger)
        {
            _bankingDbContext = bankingDbContext;
            _logger = logger;
        }

        protected static void CheckRule(IRule rule)
        {
            if (rule.IsBroken())
            {
                throw new BankingException(rule.Message, rule.ErrorCode);
            }
        }

        protected static async Task CheckRuleAsync(IAsyncRule rule)
        {
            if (await rule.IsBroken())
            {
                throw new BankingException(rule.Message, rule.ErrorCode);
            }
        }
    }
}
