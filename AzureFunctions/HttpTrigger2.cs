using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QueueMessageCreators.Models;
using System.Collections.Generic;
using System;
using System.Web.Http;
using Microsoft.WindowsAzure.Storage.Table;

namespace QueueMessageCreators.AzureFunctions
{
    public static class HttpTrigger2
    {
        [FunctionName("HttpTrigger2")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
            [Table("Users", Connection = "AzureWebJobsStorage")] CloudTable table,
            ILogger log)
        {
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var users = JsonConvert.DeserializeObject<List<User>>(requestBody);
                users.ForEach(user => user.SetPartitionAndRowKeys());

                foreach(var user in users)
                {
                    await table.ExecuteAsync(TableOperation.Insert(user));
                }

                var message = $"Users {JsonConvert.SerializeObject(users)} were successfully added to table.";
                log.LogInformation(message);

                return new OkObjectResult(message);
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                return new ExceptionResult(e, true);
            }
        }
    }
}
