using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Web.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;

namespace QueueMessageCreators
{
    public static class HttpTrigger
    {
        [FunctionName("HttpTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
            ILogger log)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var messagesToBeQueued = JsonConvert.DeserializeObject<MessagesToBeQueued>(requestBody);

            try
            {
                await PushMessagesToQueueAsync(messagesToBeQueued);
                var message = $"Messages {messagesToBeQueued.MessagesToString()} were successfully added to queue: \"{messagesToBeQueued.QueueName}\"";
                log.LogInformation(message);

                return new OkObjectResult(message);
            }
            catch(Exception e)
            {
                log.LogError(e.Message);
                return new ExceptionResult(e, true);
            }
        }

        private static async Task PushMessagesToQueueAsync(MessagesToBeQueued messagesToBeQueued)
        {
            var connectionString = Environment.GetEnvironmentVariable("CUSTOMCONNSTR_StorageConnectionString");

            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var cloudQueueClient = storageAccount.CreateCloudQueueClient();

            var queue = cloudQueueClient.GetQueueReference(messagesToBeQueued.QueueName);
            await queue.CreateIfNotExistsAsync();

            foreach (var message in messagesToBeQueued.Messages)
            {
                await queue.AddMessageAsync(new CloudQueueMessage(message));
            }
        }
    }
}
