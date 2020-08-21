using Newtonsoft.Json;

namespace Core.Lib.Middlewares.Exceptions
{
    public class ErrorDetails
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
