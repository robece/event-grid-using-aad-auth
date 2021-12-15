# event-grid-using-aad-auth

Instructions to follow:

There are multiple scenarios to authenticate using Azure Active Directory in this scenario we are using an Application with a client secret.

Make sure you already have created the Application in Azure Active Directory and the client secret generated (you will need to provide this information in the code).

In the Event Grid topic add a "role assignment" using system assigned feature in Identity.
The role assignment will provide the grants to the Application to communicate with the topic using the role: EventGrid Data Sender, otherwise the code will fail.

References: 

Azure Event Grid client library for .NET
[https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/eventgrid/Azure.Messaging.EventGrid#authenticate-using-azure-active-directory](https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/eventgrid/Azure.Messaging.EventGrid#authenticate-using-azure-active-directory)

Azure Identity client library for .NET
[https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/identity/Azure.Identity/README.md](https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/identity/Azure.Identity/README.md)
