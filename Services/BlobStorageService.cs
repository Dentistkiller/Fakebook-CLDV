namespace YourProjectName.Services
{
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using Azure.Storage.Sas;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class BlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public BlobStorageService(IConfiguration configuration)
        {
            _blobServiceClient = new BlobServiceClient(configuration["ConnectionStrings:ContainerConnection"]);
            _containerName = configuration["ConnectionStrings:ContainerName"];
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            // Get the container client
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

            // Get the blob client
            var blobClient = containerClient.GetBlobClient(fileName);

            // Upload the file
            await blobClient.UploadAsync(fileStream, overwrite: true);

            // Return the SAS token
            return GenerateSasToken(blobClient);


        }

        private string GenerateSasToken(BlobClient blobClient)
        {
            // Create a SAS builder to specify permissions and expiry
            //this create the token
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _containerName,
                BlobName = blobClient.Name,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)  // Token expiration time (adjust as needed)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            // Generate the SAS URI
            Uri sasUri = blobClient.GenerateSasUri(sasBuilder);

            return sasUri.ToString(); // <-- Return full URL, not just the query
        }
    }
}
