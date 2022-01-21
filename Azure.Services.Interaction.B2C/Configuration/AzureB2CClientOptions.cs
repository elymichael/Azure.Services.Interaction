namespace Azure.Services.Interaction.B2C.Configuration
{
    /// <summary>
    /// Azure B2C client options.
    /// </summary>
    public class AzureB2CClientOptions
    {
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TenantLoginUrl { get; set; }
        public string GraphResourceUrl { get; set; }
        public string GraphVersion { get; set; }
        public string Instance { get; set; }
        public string Domain { get; set; }
        public string SignUpSignInPolicyId { get; set; }
        public bool AllowWebApiToBeAuthorizedByACL { get; set; }
    }
}
