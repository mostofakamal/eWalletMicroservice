using Kyc.Domain.AggregateModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kyc.API.Application.Services
{
    public class ExternalKycVerifier : IExternalKycVerifier
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ExternalKycVerifier> logger;

        public ExternalKycVerifier(HttpClient httpClient, IConfiguration configuration, ILogger<ExternalKycVerifier> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            this.logger = logger;
        }
        public async Task<KycStatuses> Verify(KycVerificationRequest kycRequest, string countryName)
        {
            var externalApiUrl = _configuration[$"{countryName}_KycVerificationUrl"];
            var query = $"/?firstName={kycRequest.FirstName}&lastName={kycRequest.LastName}&nid={kycRequest.NID}";
            var request = new HttpRequestMessage(HttpMethod.Get, externalApiUrl + query);
            var response = await _httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            this.logger.Log(LogLevel.Information, $"Response :{responseString} ");

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var kycVerificationResponse = JsonConvert.DeserializeObject<IList<ExternalKycVerificationResponse>>(responseString, settings);

            return kycVerificationResponse != null && kycVerificationResponse.Any() ? KycStatuses.Approved : KycStatuses.Failed;
        }
    }
}
