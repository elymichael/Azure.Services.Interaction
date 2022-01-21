namespace Azure.Services.Interaction.RestClient.Helper.Exceptions
{
    using System;
    public class EntityNotFoundExceptions : Exception
    {
        public EntityNotFoundExceptions() : base()
        {
        }
        public EntityNotFoundExceptions(string message) : base(message)
        {
        }
    }
}
