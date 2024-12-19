## Setting Up Azure Relay

Follow these steps:

1. **Create an Azure Relay resource** on the portal.
2. **Create a hybrid connection**. Make sure to keep the check box for "Requires Client Authorization" unchecked.
3. Inside the hybrid connection, **create a new shared access policy** with the name "ListenerPolicy" and check the box for "Listen".
4. **Set up the server**:
   - Clone the repository containing the sample code for the server which listens to the Hybrid Connection Endpoint and echoes the request.
   - Add the NuGet package `Microsoft.Azure.Relay`.
   - In the `program.cs` file, add the name of your relay namespace and hybrid connection in the relevant placeholders. Finally, copy the connection string from the shared access policy that we created into the 'key' placeholder.
5. **Run the server**.
6. Use HTTPie or any other similar client to send a POST request to your "Hybrid Connection URL" with some message in the body. In the response, you'll get the message you sent in the request back.

Reference: https://learn.microsoft.com/en-us/azure/azure-relay/relay-what-is-it  
Github repository for more samples: https://github.com/Azure/azure-relay/blob/master/samples/README.md
