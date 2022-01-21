namespace Azure.Services.Interaction.Storage.Test
{
    using Azure;
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using Azure.Services.Interaction.Storage.Services;
    using Moq;
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    public class AzureBlogStorageServiceTest
    {
        private readonly Mock<BlobServiceClient> _blobServiceClientMock;

        public AzureBlogStorageServiceTest()
        {
            var blobClient = new Mock<BlobClient>();

            blobClient.Setup(m => m.DownloadContentAsync()).Returns(
                Task.FromResult(Response.FromValue<BlobDownloadResult>(BlobsModelFactory.BlobDownloadResult(), null)));
            blobClient.Setup(m => m.UploadAsync(It.IsAny<BinaryData>())).Returns(
                Task.FromResult(Response.FromValue<BlobContentInfo>(BlobsModelFactory.BlobContentInfo(ETag.All, DateTimeOffset.Now, null, null, null, null, 0), null)));
            blobClient.Setup(m => m.DeleteIfExistsAsync(DeleteSnapshotsOption.None, null, It.IsAny<CancellationToken>())).Returns(
                Task.FromResult(Response.FromValue<bool>(true, null)));

            var blobContainerClientMock = new Mock<BlobContainerClient>();
            blobContainerClientMock.Setup(m => m.GetBlobClient(It.IsAny<string>())).Returns(blobClient.Object);

            _blobServiceClientMock = new Mock<BlobServiceClient>();
            _blobServiceClientMock.Setup(m => m.GetBlobContainerClient(It.IsAny<string>())).Returns(blobContainerClientMock.Object);
        }

        [Fact]
        public async Task GetBlobAsync_Returns_Success()
        {
            var service = new AzureBlobStorageService(_blobServiceClientMock.Object);

            var ex = await Record.ExceptionAsync(() => service.GetBlobAsync("documents", "DocumentTemplates/Test"));

            Assert.Null(ex);
        }

        [Fact]
        public async Task UploadBlobAsync_Returns_Success()
        {
            var service = new AzureBlobStorageService(_blobServiceClientMock.Object);

            var file = new FileStream(
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "testDocument.txt"),
                    FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);

            var ex = await Record.ExceptionAsync(() => 
                service.UploadBlobAsync("documents", "Documents/Test", file));

            file.Close();

            Assert.Null(ex);
        }

        [Fact]
        public async Task GetAllBlobName_Returns_Success()
        {
            var service = new AzureBlobStorageService(_blobServiceClientMock.Object);
            var file = new FileStream(
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "testDocument.txt"),
                    FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);

            var ex = await Record.ExceptionAsync(() =>
                service.UploadBlobAsync("documents", "Documents/Test", file));

            file.Close();
            Assert.Null(ex);
        }

        [Fact]
        public async Task DeleteBlobAsync_Returns_Success()
        {
            var service = new AzureBlobStorageService(_blobServiceClientMock.Object);

            bool deleted = await service.DeleteBlobAsync("documents", "testDocument.txt");

            Assert.True(deleted);
        }
    }
}
