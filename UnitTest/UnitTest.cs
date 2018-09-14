using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Linq;

namespace KoenZomers.UniFi.Api.UnitTest
{
    [TestClass]
    public class UnitTest
    {
        private static Api uniFiApi;

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            uniFiApi = new Api(new Uri(ConfigurationManager.AppSettings["UniFiControllerUrl"]));
            uniFiApi.DisableSslValidation();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            uniFiApi.Dispose();
        }

        /// <summary>
        /// Tests if the authentication succeeds towards UniFi with the credentials provided in the AppConfig
        /// </summary>
        [TestMethod]
        public void AuthenticateTestMethod()
        {
            var task = uniFiApi.Authenticate(ConfigurationManager.AppSettings["UniFiControllerUsername"], ConfigurationManager.AppSettings["UniFiControllerPassword"]);
            task.Wait();
            Assert.IsTrue(task.Result, "Authentication failed");
        }

        /// <summary>
        /// Tests retrieving all UniFi devices
        /// </summary>
        [TestMethod]
        public void GetDevicesTestMethod()
        {
            if (!uniFiApi.IsAuthenticated) AuthenticateTestMethod();

            var task = uniFiApi.GetDevices();
            task.Wait();
            Assert.IsTrue(task.Result.Count > 0, "No UniFi devices found");
        }

        /// <summary>
        /// Tests retrieving all clients that are currently active on the UniFi network
        /// </summary>
        [TestMethod]
        public void GetActiveClientsTestMethod()
        {
            if (!uniFiApi.IsAuthenticated) AuthenticateTestMethod();

            var task = uniFiApi.GetActiveClients();
            task.Wait();
            Assert.IsTrue(task.Result.Count > 0, "No active clients found");
        }

        /// <summary>
        /// Tests retrieving all clients known to UniFi whether or not they're currently connected
        /// </summary>
        [TestMethod]
        public void GetAllClientsTestMethod()
        {
            if (!uniFiApi.IsAuthenticated) AuthenticateTestMethod();

            var task = uniFiApi.GetAllClients();
            task.Wait();
            Assert.IsTrue(task.Result.Count > 0, "No clients found");
        }

        /// <summary>
        /// Tests if blocking and unblocking of a client on the UniFi network works
        /// </summary>
        [TestMethod]
        public void BlockUnblockClientTestMethod()
        {
            if (!uniFiApi.IsAuthenticated) AuthenticateTestMethod();

            // First retrieve all clients
            var task1 = uniFiApi.GetAllClients();
            task1.Wait();

            // Ensure we have at least one client on the UniFi network which we can temporarily block to test the functionality
            if (task1.Result.Count == 0)
            {
                Assert.Inconclusive("No clients currently on the UniFi network to use for testing to block");
            }

            // Get the first client on the UniFi network so we can change its blocked state
            var client = task1.Result[0];

            // Ensure that we know the client its current blocked state
            if (!client.IsBlocked.HasValue)
            {
                Assert.Inconclusive($"Unable to define current blocked state on client {client.FriendlyName} which is required for this test to work");
            }

            var isInitiallyBlocked = client.IsBlocked.Value;

            // Switch the blocked state of the client
            var blockTask1 = isInitiallyBlocked ? uniFiApi.UnblockClient(client) : uniFiApi.BlockClient(client);
            blockTask1.Wait();

            // Retrieve the current state of all clients again
            var task2 = uniFiApi.GetAllClients();
            task2.Wait();

            // Filter to the same client as we switched the blocked state of
            var clientAgain = task2.Result.FirstOrDefault(c => c.MacAddress == client.MacAddress);
            if(clientAgain == null)
            {
                Assert.Inconclusive($"Unable to retrieve client {client.FriendlyName} again to validate switched blocked state");
            }

            // Validate that the current blocked state is no longer the same as it initially was, this it changed
            if(clientAgain.IsBlocked != !isInitiallyBlocked)
            {
                Assert.Fail($"Switching blocked state of client {client.FriendlyName} has not lead to it actually switching its state");
            }

            // Switch the blocked state of the client back to what it was
            var blockTask2 = isInitiallyBlocked ? uniFiApi.BlockClient(client) : uniFiApi.UnblockClient(client);
            blockTask2.Wait();

            // If the execution gets here it means the block switching was successful
        }
    }
}
