namespace Core.Lib.IntegrationEvents
{
    public class SmsIntegrationMessage : IntegrationMessage
    {
        public string DestinationPhoneNumber { get; private set; }

        public string SmsText { get; private set; }

        public SmsIntegrationMessage(string destinationPhoneNumber,string smsText)
        {
            DestinationPhoneNumber = destinationPhoneNumber;
            SmsText = smsText;
        }

    }
}