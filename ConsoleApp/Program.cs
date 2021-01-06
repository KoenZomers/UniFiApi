using System;
using System.Configuration;
using System.Threading.Tasks;

namespace KoenZomers.UniFi.Api.ConsoleApp
{
    class Program
    {
        /// <summary>
        /// Default synchronous application main
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Switch to async application main
            Task t = MainAsync(args);
            t.Wait();
        }

        /// <summary>
        /// Asynchronous application Main
        /// </summary>
        static async Task MainAsync(string[] args)
        {
            // Create a new Api instance to connect with the UniFi Controller
            Console.WriteLine("Connecting to UniFi Controller");
            var uniFiApi = new Api(new Uri(ConfigurationManager.AppSettings["UniFiControllerUrl"]), ConfigurationManager.AppSettings["UniFiControllerSiteId"]);

            // Disable SSL validation as UniFi uses a self signed certificate
            Console.WriteLine("- Disabling SSL validation");
            uniFiApi.DisableSslValidation();

            // Authenticate to UniFi
            Console.WriteLine("- Authenticating");
            var authenticationSuccessful = await uniFiApi.Authenticate(ConfigurationManager.AppSettings["UniFiControllerUsername"], ConfigurationManager.AppSettings["UniFiControllerPassword"]);

            if (!authenticationSuccessful)
            {
                Console.WriteLine("- Authentication failed");
                return;
            }

            Console.WriteLine("- Authentication successful");

            // Retrieve the UniFi devices
            Console.WriteLine("- Getting devices");
            var devices = await uniFiApi.GetDevices();

            foreach (var device in devices)
            {
                Console.WriteLine($"  - {device.Name} (MAC {device.MacAddress})");
            }

            // Retrieve the active clients
            Console.WriteLine("- Getting active clients");
            var activeClients = await uniFiApi.GetActiveClients();

            foreach (var activeClient in activeClients)
            {
                Console.WriteLine($"  - {activeClient.FriendlyName} (MAC {activeClient.MacAddress}, Channel {activeClient.Channel})");
            }

            // Logout
            await uniFiApi.Logout();
            
        }
    }
}
