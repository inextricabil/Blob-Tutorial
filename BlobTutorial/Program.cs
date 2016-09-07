using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types
using System.IO;

namespace BlobTutorial
{
    class Program
    {
        static void Main(string[] args)
        {

            // Parse the connection string and return a reference to the storage account.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container.
            CloudBlobContainer container = blobClient.GetContainerReference("students-photos-container");

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();

            // Make the files within the container available to everyone
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            CloudBlockBlob blockBlob = container.GetBlockBlobReference("myblob");

            // Get the path for all the photos from directory
            string[] photoEntries = Directory.GetFiles(@"C:\Users\Serban\Documents\visual studio 2015\Projects\BlobTutorial\BlobTutorial\Photos");

            // Create a list of Block Blobs
            List<CloudBlockBlob> CloudBlockBlobList = new List<CloudBlockBlob>();

            CloudBlockBlob currentBlob;
            string currentPhotoName;
            int fileNamePosition;

            for (int i = 0; i < photoEntries.Length; i++)
            {
                fileNamePosition = photoEntries[i].LastIndexOf("\\") + 1;
                currentPhotoName = photoEntries[i].Substring(fileNamePosition, photoEntries[i].Length - fileNamePosition);
                currentBlob = container.GetBlockBlobReference(currentPhotoName);
                CloudBlockBlobList.Add(currentBlob);
            }

            int photoIndex = 0;
            foreach (var CloudBlockBlob in CloudBlockBlobList)
            {
                using (var fileStream = File.OpenRead(photoEntries[photoIndex++]))
                {
                    CloudBlockBlob.UploadFromStream(fileStream);
                }
            }
        }
    }
}
