using Microsoft.WindowsAzure.Storage.Table;

namespace QueueMessageCreators.Models
{
    public class User : TableEntity
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public void SetPartitionAndRowKeys()
        {
            PartitionKey = Role;
            RowKey = Email;
        }
    }
}
