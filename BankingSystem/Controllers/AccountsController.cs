using BankingSystem.Application.Commands.Accounts;
using BankingSystem.Application.Queries.Accounts;
using BankingSystem.DTO.Accounts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AccountDTO>>> Get(
            [FromQuery] Guid? userId,
            [FromQuery] bool? isClosed,
            [FromQuery] int? skip,
            [FromQuery] int? take)
        {
            var result = await _mediator.Send(new GetAccountsQuery(userId, isClosed, skip, take));

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDTO>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetAccountByIdQuery(id));

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<AccountDTO>> Create(AccountDTO account)
        {
            return Ok(await _mediator.Send(new CreateAccountCommand(account)));
        }

        [HttpPut("{id}/Close")]
        public async Task<ActionResult<AccountDTO>> Close(Guid id)
        {
            return Ok(await _mediator.Send(new CloseAccountCommand(id)));
        }
    }
}
