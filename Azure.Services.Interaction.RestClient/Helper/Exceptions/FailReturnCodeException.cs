namespace Azure.Services.Interaction.RestClient.Helper.Exceptions
{
    using System;
    public class FailReturnCodeException : Exception
    {
        public FailReturnCodeException(string message) : base(message)
        {
        }

    }
}
