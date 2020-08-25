using Reward.Domain.AggregateModel;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IRewardService
    {
        Task<UserReward> CheckAndProcessReward(User customer,RewardOperation operationName);
    }
}