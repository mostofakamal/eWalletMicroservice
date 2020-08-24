using System.Threading.Tasks;
using Core.Lib.IntegrationEvents;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Reward.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RewardsController : ControllerBase
    {
        private readonly ILogger<RewardsController> _logger;
        private readonly IMediator _mediator;
        private readonly IBus bus;

        public RewardsController(ILogger<RewardsController> logger, IMediator mediator, IBus bus)
        {
            _logger = logger;
            _mediator = mediator;
            this.bus = bus;
        }

        [HttpGet]
        public void Send()
        {
            bus.Send<TransactionIntegrationMessage>(new TransactionIntegrationMessage() { Amount = 1 });
        }
    }
}
