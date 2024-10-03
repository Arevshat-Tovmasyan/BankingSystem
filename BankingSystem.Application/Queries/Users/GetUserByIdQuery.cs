using BankingSystem.Domain.Exceptions;
using BankingSystem.Domain.Interfaces;
using BankingSystem.DTO.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankingSystem.Application.Queries.Users
{
    public class GetUserByIdQuery : IQuery<GetUsersDTO>
    {
        public Guid Id { get; set; }

        public GetUserByIdQuery(Guid id)
        {
            Id = id;
        }
    }

    public class GetUserByIdQueryHandler : BaseQueryHandler, IQueryHandler<GetUserByIdQuery, GetUsersDTO>
    {
        public GetUserByIdQueryHandler(IBankingDbContext bankingDbContext, IHttpContextAccessor contextAccessor, ILogger<GetUserByIdQueryHandler> logger)
            : base(bankingDbContext, contextAccessor.HttpContext.User, logger)
        {
        }

        public async Task<GetUsersDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _bankingDbContext.Users.AsNoTracking().Select(GetUsersDTO.GetSelector()).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (user == null)
            {
                throw new BankingException($"User with the given id ({request.Id}) does not exist", ErrorCode.NotFound);
            }

            return user;
        }
    }
}
