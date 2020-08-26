using Core.Lib.IntegrationEvents;
using Kyc.API.Application.Commands;
using Kyc.API.Application.Queries;
using MassTransit;
using MassTransit.Initializers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Kyc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KycController : ControllerBase
    {
        private readonly ILogger<KycController> _logger;
        private readonly IMediator mediator;
        private readonly ISendEndpointProvider sendEndpointProvider;
        private readonly IBus _bus;
        private readonly IBusControl control;

        public KycController(ILogger<KycController> logger,
            IBus bus,
            IMediator mediator,
            ISendEndpointProvider sendEndpointProvider,
            IBusControl control)
        {
            _logger = logger;
            this.mediator = mediator;
            this.sendEndpointProvider = sendEndpointProvider;
            this.control = control;
            this._bus = bus;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitKyc(SubmitKycCommand submitKycCommand)
        {
            var kycResponse = await this.mediator.Send<SumitKycResponse>(submitKycCommand);
            return Ok(kycResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetKycInformations()
        {
            var kycHistoryResponse = await this.mediator.Send<KycHistoryResponse>(new GetKycHistoryQuery());
            return Ok(kycHistoryResponse);
        }

        [HttpGet]
        [Route("sendmessage")]
        public async Task<OkObjectResult> SendAsync()
        {
            var amount = new Random().Next();
            //var sendEndpioint = await this.control.GetSendEndpoint(new Uri("queue:TransactionIntegrationMessage"));
            await _bus.Publish<ITransactionIntegrationMessage>(new TransactionIntegrationMessage() { Amount = amount });
            return Ok(amount);
        }
    }
}
