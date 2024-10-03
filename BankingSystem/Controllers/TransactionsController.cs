using BankingSystem.Application.Commands.Transactions;
using BankingSystem.Application.Queries.Transactions;
using BankingSystem.DTO.Accounts;
using BankingSystem.DTO.Transactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AccountDTO>>> Get(
            [Required] Guid accountId,
            [FromQuery] int? skip,
            [FromQuery] int? take)
        {
            var result = await _mediator.Send(new GetTransactionsByAccountIdQuery(accountId, skip, take));

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDTO>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetTransactionByIdQuery(id));

            return Ok(result);
        }

        [HttpPost("Deposit")]
        public async Task<ActionResult<TransactionDTO>> Deposit(DepositDTO deposit)
        {
            return Ok(await _mediator.Send(new DepositCommand(deposit)));
        }

        [HttpPost("Withdraw")]
        public async Task<ActionResult<TransactionDTO>> Withdraw(WithdrawDTO withdraw)
        {
            return Ok(await _mediator.Send(new WithdrawCommand(withdraw)));
        }

        [HttpPost("Transfer")]
        public async Task<ActionResult<IReadOnlyList<TransactionDTO>>> Transfer(TransferDTO Transfer)
        {
            return Ok(await _mediator.Send(new TransferCommand(Transfer)));
        }
    }
}
