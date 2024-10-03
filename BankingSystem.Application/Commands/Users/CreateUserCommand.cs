using BankingSystem.Domain.Interfaces;
using BankingSystem.DTO.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankingSystem.Application.Commands.Users
{
    public class CreateUserCommand : ICommand<GetUsersDTO>
    {
        public UserDTO User { get; set; }

        public CreateUserCommand(UserDTO user)
        {
            User = user;
        }
    }

    public class CreateUserCommandHandler : BaseCommandHandler, ICommandHandler<CreateUserCommand, GetUsersDTO>
    {
        private readonly IUserDomainService _userDomainService;

        public CreateUserCommandHandler(IBankingDbContext bankingDbContext, IHttpContextAccessor contextAccessor, ILogger<CreateUserCommandHandler> logger, IUserDomainService userDomainService)
            : base(bankingDbContext, contextAccessor.HttpContext.User, logger)
        {
            _userDomainService = userDomainService;
        }

        public async Task<GetUsersDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = UserDTO.ToEntity(request.User);

            var createdUser = await _userDomainService.CreateAsync(user, cancellationToken);

            await _bankingDbContext.SaveChangesAsync(cancellationToken);

            var result = await _bankingDbContext.Users.AsNoTracking()
                .Select(GetUsersDTO.GetSelector())
                .FirstAsync(x => x.Id == createdUser.Id, cancellationToken);

            return result;
        }
    }
}
