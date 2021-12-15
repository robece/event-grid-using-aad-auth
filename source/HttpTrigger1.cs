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

/*
Instructions to follow:

There are multiple scenarios to authenticate using Azure Active Directory in this scenario we are using an Application with a client secret.
Make sure you already have created the Application in Azure Active Directory and the client secret generated (you will need to provide this information in the code).
In the Event Grid topic add a "role assignment" using system assigned feature in Identity.
The role assignment will provide the grants to the Application to communicate with the topic using the role: EventGrid Data Sender, otherwise the code will fail.

References: 

Azure Event Grid client library for .NET
https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/eventgrid/Azure.Messaging.EventGrid#authenticate-using-azure-active-directory

Azure Identity client library for .NET
https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/identity/Azure.Identity/README.md
*/

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
