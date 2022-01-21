namespace Azure.Services.Interaction.B2C.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class AzureB2CAdUserSearchResult
    {
        public string ObjectId { get; set; }
        public string DisplayName { get; set; }
        public bool AccountEnabled { get; set; }
        public string jobTitle { get; set; }
        public string emailAddress { get; set; }
    }
}
