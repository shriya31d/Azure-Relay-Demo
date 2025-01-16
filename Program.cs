using Microsoft.Azure.Relay;
using System.Net;

namespace Server
{
    public class Program
    {
        // replace {RelayNamespace} with the name of your namespace
        private const string RelayNamespace = "{RelayNamespace}.servicebus.windows.net";
        
        // replace {HybridConnectionName} with the name of your hybrid connection
        private const string ConnectionName = "{HybridConnectionName}";
        
        // replace {SAKKeyName} with the name of your Shared Access Policies key
        private const string KeyName = "{SAKKeyName}";

        // replace {SASKey} with the primary key
        private const string Key = "{SASKey}";

        public static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        private static async Task RunAsync()
        {
            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(KeyName, Key);
            var listener = new HybridConnectionListener(new Uri(string.Format("sb://{0}/{1}", RelayNamespace, ConnectionName)), tokenProvider);


            // Subscribe to the status events.
            listener.Connecting += (o, e) => { Console.WriteLine("Connecting"); };
            listener.Offline += (o, e) => { Console.WriteLine("Offline"); };
            listener.Online += (o, e) => { Console.WriteLine("Online"); };

            // Provide an HTTP request handler
            listener.RequestHandler = (context) =>
            {
                // The server echoes the request
                context.Response.StatusCode = HttpStatusCode.OK;
                context.Response.StatusDescription = "OK";
                context.Request.InputStream.CopyToAsync(context.Response.OutputStream);
                context.Response.Close();
            };

            // Opening the listener establishes the control channel to
            // the Azure Relay service. The control channel is continuously 
            // maintained, and is reestablished when connectivity is disrupted.
            await listener.OpenAsync();
            Console.WriteLine("Created and connected the Relay listener instance.");
            Console.WriteLine("Server listening");

            // Start a new thread that will continuously read the console.
            await Console.In.ReadLineAsync();

            // Close the listener after you exit the processing loop.
            await listener.CloseAsync();
        }
    }
}
