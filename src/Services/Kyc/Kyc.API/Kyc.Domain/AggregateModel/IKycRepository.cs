using Kyc.Domain.SeedWork;
using System.Threading.Tasks;

namespace Kyc.Domain.AggregateModel
{
    public interface IKycRepository : IRepository<KycInformation>
    {
        Task<KycInformation> Add(KycInformation kycInformation);
        Task<User> Add(User user);
        Task<KycInformation> Get(string kycId);
    }
}
