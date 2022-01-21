namespace Azure.Services.Interaction.B2C.Model
{
    /// <summary>
    /// Azure B2C Ad User Info.
    /// </summary>
    public class AzureB2CAdUserInfo
    {
        public string ObjectId { get; set; }
        public string DisplayName { get; set; }
        public bool AccountEnabled { get; set; }
    }
}
