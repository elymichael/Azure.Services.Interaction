namespace Azure.Services.Interaction.RestClient.Helper.Exceptions
{
    using System;
    public class LoanProErrorException : Exception
    {
        public LoanProErrorException(string message) : base(message)
        {
        }
    }
}
