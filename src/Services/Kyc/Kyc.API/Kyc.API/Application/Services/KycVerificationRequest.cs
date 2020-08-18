namespace Kyc.API.Application.Services
{
    public class KycVerificationRequest
    {
            public string NID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        public string CountryName { get; set; }
    }
}