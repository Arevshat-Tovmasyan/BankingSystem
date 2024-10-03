using BankingSystem.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BankingSystem.Application.Commands.Users
{
    public class DeleteUserCommand : ICommand
    {
        public Guid Id { get; set; }

        public DeleteUserCommand(Guid id)
        {
            Id = id;
        }
    }

    public class DeleteUserCommandHandler : BaseCommandHandler, ICommandHandler<DeleteUserCommand>
    {
        private readonly IUserDomainService _userDomainService;

        public DeleteUserCommandHandler(IBankingDbContext bankingDbContext, IHttpContextAccessor contextAccessor, ILogger<DeleteUserCommandHandler> logger, IUserDomainService userDomainService)
            : base(bankingDbContext, contextAccessor.HttpContext.User, logger)
        {
            _userDomainService = userDomainService;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await _userDomainService.DeleteAsync(request.Id, cancellationToken);

            await _bankingDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
