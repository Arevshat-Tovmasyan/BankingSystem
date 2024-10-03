using BankingSystem.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace BankingSystem.Application.Queries
{
    public abstract class BaseQueryHandler
    {
        protected readonly ILogger _logger;
        protected readonly ClaimsPrincipal _user;
        protected readonly Guid _currentUserId;
        protected readonly IBankingDbContext _bankingDbContext;

        protected BaseQueryHandler(IBankingDbContext bankingDbContext, ClaimsPrincipal user, ILogger logger)
        {
            _logger = logger;
            _user = user;
            _bankingDbContext = bankingDbContext;
            _currentUserId = GetCurrentUserId(user);
        }

        private static Guid GetCurrentUserId(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier) == null ? Guid.Empty : Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}
