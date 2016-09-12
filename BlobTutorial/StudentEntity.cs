using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobTutorial
{
    public class StudentEntity : TableEntity
    {
        public StudentEntity(string firstName, string lastName, string Email, string PhoneNumber, uint Age)
        {
            PartitionKey = lastName;
            RowKey = firstName;
            this.Email = Email;
            this.PhoneNumber = PhoneNumber;
            this.Age = Age;
        }

        public StudentEntity() { }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public uint Age { get; set; }
    }
}
