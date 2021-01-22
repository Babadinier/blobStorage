using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace babadinier.blobStorage
{
    class Program
    {
        private static readonly string connectionString = "<YOUR-CONNECTION-STRING>";
        
        static async Task Main(string[] args)
        {
            await CreateBlobContainer();

            System.Console.WriteLine("Done");
            System.Console.ReadLine();
        }

        private static async Task CreateBlobContainer()
        {
            // Create a BlobServiceClient object which will be used to create a container client
            var blobServiceClient = new BlobServiceClient(connectionString);

            //Create a unique name for the container
            var containerName = $"container-{Guid.NewGuid().ToString()}";

            // Create the container and return a container client object
            var containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
        }
    }
}
