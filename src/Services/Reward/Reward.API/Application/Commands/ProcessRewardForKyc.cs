using MediatR;
using System;

namespace Reward.API.Application.Commands
{
    public class ProcessRewardForKyc : IRequest<bool>
    {
        public Guid UserId { get; set; }
    }
}
