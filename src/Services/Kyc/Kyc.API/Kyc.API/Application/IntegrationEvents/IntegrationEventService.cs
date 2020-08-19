using Core.Lib.IntegrationEvents;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kyc.API.Application.IntegrationEvents
{
    public class IntegrationEventService
    {
        private List<IItegration> integrations;
        public IntegrationEventService()
        {
            integrations = new List<IItegration>();
        }
        public void AddAndSaveEventAsync(IItegration evt)
        {
            integrations.Add(evt);
        }
    }
}
