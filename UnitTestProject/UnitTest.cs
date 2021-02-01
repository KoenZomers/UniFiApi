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
            var siteId = ConfigurationManager.AppSettings["UniFiControllerSiteId"];
            var controllerUri = new Uri(ConfigurationManager.AppSettings["UniFiControllerUrl"]);

            uniFiApi = string.IsNullOrWhiteSpace(siteId) ? new Api(controllerUri) : new Api(controllerUri, siteId);
            uniFiApi.DisableSslValidation();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            
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
        /// Tests if the authentication fails with the expected outcome if we provide incorrect credentials towards UniFi
        /// </summary>
        [TestMethod]
        public void AuthenticateFailsTestMethod()
        {
            var task = uniFiApi.Authenticate("doesnotexist", "doesnotexist");
            task.Wait();
            Assert.IsFalse(task.Result, "Authentication succeeded while it should have failed");
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
        /// Tests retrieving all sites registered in UniFi
        /// </summary>
        [TestMethod]
        public void GetAllSitesTestMethod()
        {
            if (!uniFiApi.IsAuthenticated) AuthenticateTestMethod();

            var task = uniFiApi.GetSites();
            task.Wait();
            Assert.IsTrue(task.Result.Count > 0, "No sites found");
        }

        /// <summary>
        /// Tests retrieving the last 2 connections from a client in UniFi
        /// </summary>
        [TestMethod]
        public void GetClientHistoryTestMethod()
        {
            if (!uniFiApi.IsAuthenticated) AuthenticateTestMethod();

            // First retrieve all clients
            var task1 = uniFiApi.GetAllClients();
            task1.Wait();

            // Ensure we have at least one client on the UniFi network which we can retrieve the history of to test the functionality
            if (task1.Result.Count == 0)
            {
                Assert.Inconclusive("No clients currently on the UniFi network to use for testing to retrieve the connection history");
            }

            // Get the first client on the UniFi network so we can retrieve the connection history
            var client = task1.Result[0];

            var task = uniFiApi.GetClientHistory(client.MacAddress, 2);
            task.Wait();
            Assert.IsTrue(task.Result.Count == 2, "Not the expected amount of historical items found");
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
            if (clientAgain == null)
            {
                Assert.Inconclusive($"Unable to retrieve client {client.FriendlyName} again to validate switched blocked state");
            }

            // Validate that the current blocked state is no longer the same as it initially was, this it changed
            if (clientAgain.IsBlocked != !isInitiallyBlocked)
            {
                Assert.Fail($"Switching blocked state of client {client.FriendlyName} has not lead to it actually switching its state");
            }

            // Switch the blocked state of the client back to what it was
            var blockTask2 = isInitiallyBlocked ? uniFiApi.BlockClient(client) : uniFiApi.UnblockClient(client);
            blockTask2.Wait();

            // If the execution gets here it means the block switching was successful
        }

        /// <summary>
        /// Tests if renaming of a client on the UniFi network works
        /// </summary>
        [TestMethod]
        public void RenameClientTestMethod()
        {
            if (!uniFiApi.IsAuthenticated) AuthenticateTestMethod();

            // First retrieve all clients
            var task1 = uniFiApi.GetAllClients();
            task1.Wait();

            // Ensure we have at least one client on the UniFi network which we can temporarily rename to test the functionality
            if (task1.Result.Count == 0)
            {
                Assert.Inconclusive("No clients currently on the UniFi network to use for testing to rename");
            }

            // Get the first client on the UniFi network so we can change its name
            var client = task1.Result[0];

            // Ensure that we know the client its current name
            var currentName = client.FriendlyName;
            var temporaryName = string.Concat(currentName, " - Rename Test");

            // Rename the client
            var renameTask1 = uniFiApi.RenameClient(client, temporaryName);
            renameTask1.Wait();

            // Retrieve the current state of all clients again
            var task2 = uniFiApi.GetAllClients();
            task2.Wait();

            // Filter to the same client as we renamed
            var clientAgain = task2.Result.FirstOrDefault(c => c.MacAddress == client.MacAddress);
            if (clientAgain == null)
            {
                Assert.Inconclusive($"Unable to retrieve client {client.FriendlyName} again to validate new name");
            }

            // Validate that the current name is the updated name
            if (clientAgain.FriendlyName != temporaryName)
            {
                Assert.Fail($"Renaming client did not succeed. Name before: {currentName}. Name after: {clientAgain.FriendlyName}. Expected name: {temporaryName}.");
            }

            // Rename the client back to its original name
            var renameTask2 = uniFiApi.RenameClient(client, currentName);
            renameTask2.Wait();

            // If the execution gets here it means the rename was successful
        }

        /// <summary>
        /// Tests if authorizing and unauthorizing of a guest on the UniFi network works
        /// </summary>
        [TestMethod]
        public void AuthorizeUnAuthorizeGuestTestMethod()
        {
            if (!uniFiApi.IsAuthenticated) AuthenticateTestMethod();

            // First retrieve all active clients
            var task1 = uniFiApi.GetActiveClients();
            task1.Wait();

            // Ensure we have at least one client on the UniFi network which we can temporarily block to test the functionality
            if (task1.Result.Count == 0)
            {
                Assert.Inconclusive("No clients currently on the UniFi network to use for testing to block");
            }

            // Get the first guest client on the UniFi network so we can change its authorized state
            var client = task1.Result.FirstOrDefault(c => c.IsGuest.GetValueOrDefault(false));

            // Ensure that we have a guest client
            if (client == null)
            {
                Assert.Inconclusive($"Unable to find a guest on your network which is required for this test to work");
            }

            // Get the current authorized state of the guest
            bool guestInitiallyAuthorized = client.IsAuthorized.GetValueOrDefault(false);

            // Switch the authorized state of the guest
            var task2 = guestInitiallyAuthorized ? uniFiApi.UnauthorizeGuest(client.MacAddress) : uniFiApi.AuthorizeGuest(client.MacAddress);
            task2.Wait();

            // Retrieve the active clients again
            var task3 = uniFiApi.GetActiveClients();
            task3.Wait();

            // Filter to the same client as we switched the authorization state of
            var clientAgain = task3.Result.FirstOrDefault(c => c.MacAddress == client.MacAddress);
            if (clientAgain == null)
            {
                Assert.Inconclusive($"Unable to retrieve client {client.FriendlyName} again to validate switched authorization state");
            }

            // Validate that the current authorized state is no longer the same as it initially was, this it changed
            if (clientAgain.IsAuthorized != !guestInitiallyAuthorized)
            {
                Assert.Fail($"Switching authorization state of client {client.FriendlyName} has not lead to it actually switching its state");
            }

            // Switch the blocked state of the client back to what it was
            var task4 = guestInitiallyAuthorized ? uniFiApi.AuthorizeGuest(client.MacAddress) : uniFiApi.UnauthorizeGuest(client.MacAddress);
            task4.Wait();

            // If the execution gets here it means the block switching was successful
        }

        /// <summary>
        /// Tests retrieving all networks registered in UniFi
        /// </summary>
        [TestMethod]
        public void GetNetworksTestMethod()
        {
            if (!uniFiApi.IsAuthenticated) AuthenticateTestMethod();

            var task = uniFiApi.GetNetworks();
            task.Wait();
            Assert.IsTrue(task.Result.Count > 0, "No networks found");
        }

        /// <summary>
        /// Tests retrieving all wireless networks registered in UniFi
        /// </summary>
        [TestMethod]
        public void GetWirelessNetworksTestMethod()
        {
            if (!uniFiApi.IsAuthenticated) AuthenticateTestMethod();

            var task = uniFiApi.GetWirelessNetworks();
            task.Wait();
            Assert.IsTrue(task.Result.Count > 0, "No wireless networks found");
        }
    }
}
