using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace StudentManager
{
    public class StudentManager
    {
        // Parse the connection string and return a reference to the storage account.
        private static CloudStorageAccount storageAccount;

        // Create the blob client.
        private static CloudBlobClient blobClient;

        // Create the table client.
        private static CloudTableClient tableClient;

        // Retrieve a reference to a container.
        private static CloudBlobContainer container;

        // Retrive a reference to the table.
        private static CloudTable table;


        public StudentManager()
        {
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            blobClient = storageAccount.CreateCloudBlobClient();

            tableClient = storageAccount.CreateCloudTableClient();

            container = blobClient.GetContainerReference("students-photos-container");

            container.CreateIfNotExists();

            table = tableClient.GetTableReference("students");

            table.CreateIfNotExists();
        }

    }
}
