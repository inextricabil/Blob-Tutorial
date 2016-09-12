using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types
using System.IO;

namespace StudentManager
{
    class Program
    {
        static void Main(string[] args)
        {

            // Parse the connection string and return a reference to the storage account.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to a container.
            CloudBlobContainer container = blobClient.GetContainerReference("students-photos-container");

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();

            // Retrive a reference to the table.
            CloudTable table = tableClient.GetTableReference("students");

            // Create the table if it doesn't exist.
            table.CreateIfNotExists();

            // Create a new student entities.
            StudentEntity student1 = new StudentEntity("Serban", "Boghiu", "serban_boghiu@yahoo.com", "0748882907", 20);
            StudentEntity student2 = new StudentEntity("Mircea", "Mihai", "mircea_mihai@yahoo.com", "0748882906", 20);
            StudentEntity student3 = new StudentEntity("Razvan", "Damian", "razvan_damian@yahoo.com", "0748882905", 20);
            StudentEntity student4 = new StudentEntity("Andrei", "Rachita", "andrei_rachita@yahoo.com", "0748882904", 20);
            StudentEntity student5 = new StudentEntity("Valeriu", "Costandache", "valeriu_constandache@yahoo.com", "0748882903", 20);
            StudentEntity student6 = new StudentEntity("Bogdan", "Zugravu", "bogdan_zugravu@yahoo.com", "0748882902", 20);
            StudentEntity student7 = new StudentEntity("Ionut", "Damian", "ionut_damian@yahoo.com", "0748882901", 20);
            StudentEntity student8 = new StudentEntity("Marius", "Andronic", "marius_andronic@yahoo.com", "0748882900", 20);
            StudentEntity student9 = new StudentEntity("Mircea", "Hortensiu", "mircea_hortensiu@yahoo.com", "0748882910", 20);
            StudentEntity student10 = new StudentEntity("Ilie", "Nastase", "ilie_nastase@yahoo.com", "0748882911", 20);

            // Add each student to the table.
            AddStudent(student1, table);
            AddStudent(student2, table);
            AddStudent(student3, table);
            AddStudent(student4, table);
            AddStudent(student5, table);
            AddStudent(student6, table);
            AddStudent(student7, table);
            AddStudent(student8, table);
            AddStudent(student9, table);
            AddStudent(student10, table);

            // Make the files within the container available to everyone
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

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

        public static void AddStudent(StudentEntity student, CloudTable table)
        {
            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(student);

            // Execute the insert operation.
            table.Execute(insertOperation);
        }

        public static void DeleteStudent(string firstName, string lastName)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<StudentEntity>(firstName, lastName);

            TableResult retrievedResult = table.Execute(retrieveOperation);
        }

        public static void ReadStudents()
        {

        }


        async public static Task ListBlobsSegmentedInFlatListing(CloudBlobContainer container)
        {
            //List blobs to the console window, with paging.
            Console.WriteLine("List blobs in pages:");

            int i = 0;
            BlobContinuationToken continuationToken = null;
            BlobResultSegment resultSegment = null;

            //Call ListBlobsSegmentedAsync and enumerate the result segment returned, while the continuation token is non-null.
            //When the continuation token is null, the last page has been returned and execution can exit the loop.
            do
            {
                //This overload allows control of the page size. You can return all remaining results by passing null for the maxResults parameter,
                //or by calling a different overload.
                resultSegment = await container.ListBlobsSegmentedAsync("", true, BlobListingDetails.All, 10, continuationToken, null, null);
                if (resultSegment.Results.Count<IListBlobItem>() > 0) { Console.WriteLine("Page {0}:", ++i); }
                foreach (var blobItem in resultSegment.Results)
                {
                    Console.WriteLine("\t{0}", blobItem.StorageUri.PrimaryUri);
                }
                Console.WriteLine();

                //Get the continuation token.
                continuationToken = resultSegment.ContinuationToken;
            }
            while (continuationToken != null);
        }
    }
}
