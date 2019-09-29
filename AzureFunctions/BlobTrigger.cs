using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace QueueMessageCreators.AzureFunctions
{
    public static class BlobTrigger
    {
        [FunctionName("BlobTrigger")]
        public static async Task Run(
            [BlobTrigger("blobs-for-blob-trigger/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob,
            [Queue("blobtriggerqueue", Connection = "AzureWebJobsStorage")] IAsyncCollector<string> queue,
            string name, 
            ILogger log)
        {
            var message = $"C# Blob trigger function Processed blob:\nName: {name}, Size: {myBlob.Length} Bytes";
            await queue.AddAsync(message);
            log.LogInformation($"Message: \"{message}\" was successfully added to queue: \"timertriggerqueue\"");
        }
    }
}
