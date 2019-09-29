using Newtonsoft.Json;
using System.Collections.Generic;

namespace QueueMessageCreators
{
    public class MessagesToBeQueued
    {
        public string QueueName { get; set; }

        public List<string> Messages { get; set; }

        public string MessagesToString()
        {
            return JsonConvert.SerializeObject(Messages);
        }
    }
}
