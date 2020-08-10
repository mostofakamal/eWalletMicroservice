using Kyc.Domain.AggregateModel;
using Kyc.Domain.SeedWork;
using System;
using System.Threading.Tasks;

namespace Kyc.Insfrastructure
{
    public class KycRepository : IKycRepository
    {
        private readonly KycContext _context;

        public IUnitOfWork UnitOfWork { get { return _context; } }


        public KycRepository(KycContext kycContext)
        {
            _context = kycContext;
        }

        public async Task<KycInformation> Add(KycInformation kycInformation)
        {
            var added = await _context.Kycs.AddAsync(kycInformation);
            return added.Entity;
        }

        public async Task<KycInformation> Get(string kycId)
        {
            return await _context.Kycs.FindAsync(kycId);
        }
    }
}
