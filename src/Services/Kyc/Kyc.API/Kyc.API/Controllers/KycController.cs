using Kyc.API.Application.Commands;
using Kyc.API.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Kyc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KycController : ControllerBase
    {
        private readonly ILogger<KycController> _logger;
        private readonly IMediator mediator;

        public KycController(ILogger<KycController> logger, IMediator mediator )
        {
            _logger = logger;
            this.mediator = mediator;
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
    }
}
