namespace Azure.Services.Interaction.RestClient.Helper.Exceptions
{
    using System;
    public class DeserializationException : Exception
    {
        public DeserializationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
