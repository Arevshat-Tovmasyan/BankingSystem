using BankingSystem.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace BankingSystem.Application.Commands
{
    public abstract class BaseCommandHandler
    {
        protected readonly ILogger _logger;
        protected readonly IBankingDbContext _bankingDbContext;
        protected readonly ClaimsPrincipal _user;
        protected readonly Guid _currentUserId;
        protected readonly string _currentUserName;

        protected BaseCommandHandler(IBankingDbContext bankingDbContext, ClaimsPrincipal user, ILogger logger)
        {
            _logger = logger;
            _bankingDbContext = bankingDbContext;
            _user = user;
            _currentUserId = GetCurrentUserId(_user);
            _currentUserName = GetCurrentUserName(_user);
        }

        protected static Guid GetCurrentUserId(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier) == null ? Guid.Empty : Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        protected static string GetCurrentUserName(ClaimsPrincipal user)
        {
            return user.FindFirst("name") == null ? string.Empty : user.FindFirst("name").Value;
        }
    }
}
