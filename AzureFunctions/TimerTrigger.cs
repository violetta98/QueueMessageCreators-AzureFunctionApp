using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace QueueMessageCreators.AzureFunctions
{
    public static class TimerTrigger
    {
        [FunctionName("TimerTrigger")]
        public static async Task Run(
            [TimerTrigger("0 55 23 * * *")]TimerInfo myTimer,
            [Queue("timertriggerqueue", Connection = "AzureWebJobsStorage")] IAsyncCollector<string> queue,
            ILogger log)
        {
            var message = $"C# Timer trigger function executed at: {DateTime.Now}";
            await queue.AddAsync(message);
            log.LogInformation($"Message: \"{message}\" was successfully added to queue: \"timertriggerqueue\"");
        }
    }
}
