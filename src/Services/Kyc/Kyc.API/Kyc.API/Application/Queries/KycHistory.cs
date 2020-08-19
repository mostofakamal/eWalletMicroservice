using System;

namespace Kyc.API.Application.Queries
{
    public class KycHistory
    {
        public string NID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string KycStatus { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}