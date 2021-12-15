using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Azure.Messaging;
using Azure.Messaging.EventGrid;
using Azure;
using Azure.Identity;

namespace Company.Function
{
    public static class HttpTrigger1
    {
        [FunctionName("HttpTrigger1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            Response response = null;

            try
            {
                string topicEndpoint = "";
                Environment.SetEnvironmentVariable("AZURE_CLIENT_ID", "");
                Environment.SetEnvironmentVariable("AZURE_TENANT_ID", "");
                Environment.SetEnvironmentVariable("AZURE_CLIENT_SECRET", "");
                
                EventGridPublisherClient client = new EventGridPublisherClient(new Uri(topicEndpoint), new DefaultAzureCredential());

                // Add CloudEvents to a list to publish to the topic
                List<CloudEvent> eventsList = new List<CloudEvent>
                {
                    // CloudEvent with custom model serialized to JSON using a custom serializer
                    new CloudEvent(
                        "/cloudevents/example/source",
                        "Example.EventType",
                        "my-custom-data"),
                };

                // Send the events
                response = await client.SendEventsAsync(eventsList);
                Console.WriteLine(response);
            }
            catch
            {
                throw;
            }

            return new OkResult();
        }
    }
}
