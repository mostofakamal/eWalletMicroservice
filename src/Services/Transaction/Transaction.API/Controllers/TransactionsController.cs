﻿using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Transaction.API.Application.Commands;

namespace Transaction.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ILogger<TransactionsController> _logger;
        private readonly IMediator _mediator;

        public TransactionsController(ILogger<TransactionsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction(CreateTransactionCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }


        [HttpPost]
        [Route("transfers")]
        public async Task<IActionResult> TransferMoney(TransferMoneyCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet]
        [Route("balance")]
        public async Task<IActionResult> GetBalance()
        {
            var balanceResponse = await _mediator.Send(new GetBalanceQuery());
            return Ok(balanceResponse);
        }

    }
}
