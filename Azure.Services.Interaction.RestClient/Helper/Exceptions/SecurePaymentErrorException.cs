namespace Azure.Services.Interaction.RestClient.Helper.Exceptions
{
    using System;
    public class SecurePaymentErrorException : Exception
    {
        public SecurePaymentErrorException(string message) : base(message)
        {
        }
    }
}
