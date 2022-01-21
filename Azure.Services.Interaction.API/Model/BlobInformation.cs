namespace Azure.Services.Interaction.API.Model
{
    public class BlobInformation
    {
        public string ContainerName { get; set; }
        public string BlobName { get; set; }
        public string Based64Content { get; set; }
    }
}
