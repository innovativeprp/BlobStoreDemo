using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStoreDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var conn = ConfigurationManager.AppSettings[AppConstants.StorageConnectionString];
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get(AppConstants.StorageConnectionString));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("profiles");
              
                 container.CreateIfNotExists();

                // UploadDataToBlob(container);
                // ListBlobAttributes(container);
                // SetCustomBlobMetadata(container);
                // ListCustomMetadata(container);
                //CopyBlobs(container);
                UploadProfilesinSubDirectories(container);
                Console.WriteLine("Done");
                Console.ReadLine();
            }
           catch(Exception e)
            {
                Console.WriteLine($"Error : {e.Message}");
            }
            
        }

        private static void UploadDataToBlob(CloudBlobContainer container)
        {
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("praveen");
            using (var fileStream = System.IO.File.OpenRead(@"")) //Put the file you need to upload.
            {
                blockBlob.UploadFromStream(fileStream);
            }
        }

        private static void ListBlobAttributes(CloudBlobContainer container)
        {
            container.FetchAttributes();
            Console.WriteLine($"Name of container - {container.Name}");
            Console.WriteLine($"Storage uri - {container.StorageUri.PrimaryUri}");
           
        }

        private static void SetCustomBlobMetadata(CloudBlobContainer container)
        {
            container.Metadata.Clear();
            container.Metadata.Add("Author", "Praveen");
            container.Metadata.Add("CreatedDate", DateTime.Now.ToShortDateString());
            container.SetMetadata();
           
        }

        private static void ListCustomMetadata(CloudBlobContainer container)
        {
            foreach (var item in container.Metadata)
            {
                Console.WriteLine($"Key : {item.Key}");
                Console.WriteLine($"Value : {item.Value}\n\n");
            }
        }

        private static void CopyBlobs(CloudBlobContainer container)
        {
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("praveen");
            CloudBlockBlob copyblockBlob = container.GetBlockBlobReference("praveen-copy");
            copyblockBlob.StartCopyAsync(new Uri(blockBlob.Uri.AbsoluteUri));

        }

        private static void UploadProfilesinSubDirectories(CloudBlobContainer container)
        {
            CloudBlobDirectory mainDirectory = container.GetDirectoryReference("Info");
            CloudBlobDirectory subDirectory = mainDirectory.GetDirectoryReference("Resumes");
            CloudBlockBlob cloudBlockBlob = subDirectory.GetBlockBlobReference("praveen");

            using (var fileStream = System.IO.File.OpenRead(@"")) //put the file you need to upload.
            {
                cloudBlockBlob.UploadFromStream(fileStream);
            }
        }
    }
}
