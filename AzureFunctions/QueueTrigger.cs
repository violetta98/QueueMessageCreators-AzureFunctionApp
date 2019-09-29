using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace QueueMessageCreators.AzureFunctions
{
    public static class QueueTrigger
    {
        [FunctionName("QueueTrigger")]
        public static async Task Run(
            [QueueTrigger("messagesforqueuetrigger", Connection = "AzureWebJobsStorage")]string myQueueItem,
            [Queue("queuetriggerqueue", Connection = "AzureWebJobsStorage")] IAsyncCollector<string> queue,
            ILogger log)
        {
            var message = $"C# Queue trigger function processed: {myQueueItem}";
            await queue.AddAsync(message);
            log.LogInformation($"Message: \"{message}\" was successfully added to queue: \"queuetriggerqueue\"");
        }
    }
}
