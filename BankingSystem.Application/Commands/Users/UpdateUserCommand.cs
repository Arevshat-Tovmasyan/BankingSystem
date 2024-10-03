using BankingSystem.Domain.Interfaces;
using BankingSystem.DTO.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankingSystem.Application.Commands.Users
{
    public class UpdateUserCommand : ICommand<GetUsersDTO>
    {
        public UserDTO User { get; set; }

        public UpdateUserCommand(UserDTO user)
        {
            User = user;
        }
    }

    public class UpdateUserCommandHandler : BaseCommandHandler, ICommandHandler<UpdateUserCommand, GetUsersDTO>
    {
        private readonly IUserDomainService _userDomainService;

        public UpdateUserCommandHandler(IBankingDbContext bankingDbContext, IHttpContextAccessor contextAccessor, ILogger<UpdateUserCommandHandler> logger, IUserDomainService userDomainService)
            : base(bankingDbContext, contextAccessor.HttpContext.User, logger)
        {
            _userDomainService = userDomainService;
        }

        public async Task<GetUsersDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = UserDTO.ToEntity(request.User);

            var updatedUser = await _userDomainService.UpdateAsync(user, cancellationToken);

            await _bankingDbContext.SaveChangesAsync(cancellationToken);

            var result = await _bankingDbContext.Users.AsNoTracking()
                .Select(GetUsersDTO.GetSelector())
                .FirstAsync(x => x.Id == updatedUser.Id, cancellationToken);

            return result;
        }
    }
}
