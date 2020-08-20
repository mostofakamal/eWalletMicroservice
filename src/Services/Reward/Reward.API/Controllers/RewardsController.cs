using System.Threading.Tasks;
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

        public RewardsController(ILogger<RewardsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
    }
}
