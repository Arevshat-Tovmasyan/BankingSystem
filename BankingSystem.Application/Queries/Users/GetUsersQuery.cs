using BankingSystem.Application.Behaviours;
using BankingSystem.Domain.Interfaces;
using BankingSystem.DTO.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankingSystem.Application.Queries.Users
{
    public class GetUsersQuery : IValidationBehaviour, IQuery<IReadOnlyList<GetUsersDTO>>
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }

        public GetUsersQuery(int? skip, int? take)
        {
            Skip = skip;
            Take = take;
        }
    }

    public class GetUsersQueryHandler : BaseQueryHandler, IQueryHandler<GetUsersQuery, IReadOnlyList<GetUsersDTO>>
    {
        public GetUsersQueryHandler(IBankingDbContext bankingDbContext, IHttpContextAccessor contextAccessor, ILogger<GetUsersQueryHandler> logger) 
            : base(bankingDbContext, contextAccessor.HttpContext.User, logger)
        {
        }

        public async Task<IReadOnlyList<GetUsersDTO>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var usersQuery = _bankingDbContext.Users.AsNoTracking()
                                  .Select(GetUsersDTO.GetSelector());

            if (request.Skip.HasValue)
            {
                usersQuery = usersQuery.Skip(request.Skip.Value);
            }

            if (request.Take.HasValue)
            {
                usersQuery = usersQuery.Take(request.Take.Value);
            }

            var users = await usersQuery.ToArrayAsync(cancellationToken);

            return users;
        }
    }
}
