using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace babadinier.blobStorage
{
    class Program
    {
        private static readonly string connectionString = "<YOUR-CONNECTION-STRING>";
        private static readonly string containerName = "container-b88c3a48-69c3-4c18-a39d-2caf9caca5bd";
        private static readonly string localDataPath = "./data/";
        private static readonly string uploadFileName = "example.txt";
        private static readonly string downloadedFileName = "downloaded.txt";

        static async Task Main(string[] args)
        {
            var blobContainerClient = GetBlobContainerClient(containerName);
            if (!blobContainerClient.Exists())
            {
                blobContainerClient = await CreateBlobContainer(containerName);
            }

            var localFilePath = Path.Combine(localDataPath, uploadFileName);
            await UploadFile(blobContainerClient, localFilePath);

            var outputFilePath = Path.Combine(localDataPath, downloadedFileName);
            await DownloadFile(blobContainerClient, uploadFileName, outputFilePath);

            System.Console.WriteLine("Done");
            System.Console.ReadLine();
        }

        private static async Task<BlobContainerClient> CreateBlobContainer(string containerName)
        {
            // Create a BlobServiceClient object which will be used to create a container client
            var blobServiceClient = new BlobServiceClient(connectionString);

            // Create the container and return a container client object
            return await blobServiceClient.CreateBlobContainerAsync(containerName);
        }

        private static BlobContainerClient GetBlobContainerClient(string containerName)
        {
            var serviceClient = new BlobServiceClient(connectionString);
            return serviceClient.GetBlobContainerClient(containerName);
        }

        private static async Task UploadFile(BlobContainerClient containerClient, string filePath)
        {
            // Get a reference to a blob
            var blobClient = containerClient.GetBlobClient(uploadFileName);

            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

            // Open the file and upload its data
            using FileStream uploadFileStream = File.OpenRead(filePath);
            await blobClient.UploadAsync(uploadFileStream, true);
            uploadFileStream.Close();
        }

        private static async Task DownloadFile(BlobContainerClient containerClient, string fileNameToDownload, string outputFilePath)
        {
            var blobClient = containerClient.GetBlobClient(fileNameToDownload);

            // Download the blob's contents and save it to a file
            BlobDownloadInfo download = await blobClient.DownloadAsync();

            Console.WriteLine("\nDownloading blob to\n\t{0}\n", outputFilePath);

            using (FileStream downloadFileStream = File.OpenWrite(outputFilePath))
            {
                await download.Content.CopyToAsync(downloadFileStream);
                downloadFileStream.Close();
            }
        }
    }
}
