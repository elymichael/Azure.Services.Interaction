namespace Azure.Services.Interaction.RestClient.Helper.Exceptions
{
    using System;
    public class TimeOutException : Exception
    {
        public TimeOutException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
