using BankingSystem.Application.Commands.Users;
using BankingSystem.Application.Queries.Users;
using BankingSystem.DTO.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<GetUsersDTO>>> Get(
            [FromQuery] int? skip,
            [FromQuery] int? take)
        {
            var result = await _mediator.Send(new GetUsersQuery(skip, take));

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetUsersDTO>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(id));

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<GetUsersDTO>> Create(UserDTO user)
        {
            return Ok(await _mediator.Send(new CreateUserCommand(user)));
        }

        [HttpPut]
        public async Task<ActionResult<GetUsersDTO>> Update(UserDTO user)
        {
            return Ok(await _mediator.Send(new UpdateUserCommand(user)));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteUserCommand(id));

            return Ok();
        }
    }
}
