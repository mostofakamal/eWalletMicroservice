using Reward.Domain.AggregateModel;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IRewardService
    {
        Task<RewardRule> GetRewardRule(User user, RewardOperation operationName);
        Task<User> GetCountryAdmin(User user);
    }
}