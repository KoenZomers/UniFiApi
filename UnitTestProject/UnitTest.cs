using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace KoenZomers.UniFi.Api.UnitTest;

[TestClass]
public class UnitTest
{
    private static Api uniFiApi;

    [ClassInitialize]
    public static void ClassInit(TestContext testContext)
    {
        var siteId = ConfigurationManager.AppSettings["UniFiControllerSiteId"];
        var controllerUri = new Uri(ConfigurationManager.AppSettings["UniFiControllerUrl"]);

        uniFiApi = string.IsNullOrWhiteSpace(siteId) ? new Api(controllerUri) : new Api(controllerUri, siteId: siteId);
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        
    }

    /// <summary>
    /// Tests if the authentication succeeds towards UniFi with the credentials provided in the AppConfig
    /// </summary>
    [TestMethod]
    public async Task AuthenticateTestMethod()
    {
        var task = await uniFiApi.Authenticate(ConfigurationManager.AppSettings["UniFiControllerUsername"], ConfigurationManager.AppSettings["UniFiControllerPassword"]);
        Assert.IsTrue(task, "Authentication failed");
    }

    /// <summary>
    /// Tests if the authentication fails with the expected outcome if we provide incorrect credentials towards UniFi
    /// </summary>
    [TestMethod]
    public async Task AuthenticateFailsTestMethod()
    {
        var task = await uniFiApi.Authenticate("doesnotexist", "doesnotexist");
        Assert.IsFalse(task, "Authentication succeeded while it should have failed");
    }

    /// <summary>
    /// Tests retrieving all UniFi devices
    /// </summary>
    [TestMethod]
    public async Task GetDevicesTestMethod()
    {
        if (!uniFiApi.IsAuthenticated) await AuthenticateTestMethod();

        var task = await uniFiApi.GetDevices();
        Assert.IsTrue(task.Count > 0, "No UniFi devices found");
    }

    /// <summary>
    /// Tests retrieving all clients that are currently active on the UniFi network
    /// </summary>
    [TestMethod]
    public async Task GetActiveClientsTestMethod()
    {
        if (!uniFiApi.IsAuthenticated) await AuthenticateTestMethod();
        var task = await uniFiApi.GetActiveClients();
        Assert.IsTrue(task.Count > 0, "No active clients found");
    }

    /// <summary>
    /// Tests retrieving all clients known to UniFi whether or not they're currently connected
    /// </summary>
    [TestMethod]
    public async Task GetAllClientsTestMethod()
    {
        if (!uniFiApi.IsAuthenticated) await AuthenticateTestMethod();

        var task = await uniFiApi.GetAllClients();
        Assert.IsTrue(task.Count > 0, "No clients found");
    }

    /// <summary>
    /// Tests retrieving all sites registered in UniFi
    /// </summary>
    [TestMethod]
    public async Task GetAllSitesTestMethod()
    {
        if (!uniFiApi.IsAuthenticated) await AuthenticateTestMethod();

        var task = await uniFiApi.GetSites();
        Assert.IsTrue(task.Count > 0, "No sites found");
    }

    /// <summary>
    /// Tests retrieving the last 2 connections from a client in UniFi
    /// </summary>
    [TestMethod]
    public async Task GetClientHistoryTestMethod()
    {
        if (!uniFiApi.IsAuthenticated) await AuthenticateTestMethod();

        // First retrieve all clients
        var task1 = await uniFiApi.GetAllClients();

        // Ensure we have at least one client on the UniFi network which we can retrieve the history of to test the functionality
        if (task1.Count == 0)
        {
            Assert.Inconclusive("No clients currently on the UniFi network to use for testing to retrieve the connection history");
        }

        // Get the first client on the UniFi network so we can retrieve the connection history
        var client = task1[0];

        var task = await uniFiApi.GetClientHistory(client.MacAddress, 2);
        Assert.IsTrue(task.Count == 2, "Not the expected amount of historical items found");
    }

    /// <summary>
    /// Tests if blocking and unblocking of a client on the UniFi network works
    /// </summary>
    [TestMethod]
    public async Task BlockUnblockClientTestMethod()
    {
        if (!uniFiApi.IsAuthenticated) await AuthenticateTestMethod();

        // First retrieve all clients
        var task1 = await uniFiApi.GetAllClients();

        // Ensure we have at least one client on the UniFi network which we can temporarily block to test the functionality
        if (task1.Count == 0)
        {
            Assert.Inconclusive("No clients currently on the UniFi network to use for testing to block");
        }

        // Get the first client on the UniFi network so we can change its blocked state
        var client = task1[0];

        // Ensure that we know the client its current blocked state
        if (!client.IsBlocked.HasValue)
        {
            Assert.Inconclusive($"Unable to define current blocked state on client {client.FriendlyName} which is required for this test to work");
        }

        var isInitiallyBlocked = client.IsBlocked.Value;

        // Switch the blocked state of the client
        var blockTask1 = isInitiallyBlocked ? await uniFiApi.UnblockClient(client) : await uniFiApi.BlockClient(client);

        // Retrieve the current state of all clients again
        var task2 = await uniFiApi.GetAllClients();

        // Filter to the same client as we switched the blocked state of
        var clientAgain = task2.FirstOrDefault(c => c.MacAddress == client.MacAddress);
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
        var blockTask2 = isInitiallyBlocked ? await uniFiApi.BlockClient(client) : await uniFiApi.UnblockClient(client);

        // If the execution gets here it means the block switching was successful
    }

    /// <summary>
    /// Tests if renaming of a client on the UniFi network works
    /// </summary>
    [TestMethod]
    public async Task RenameClientTestMethod()
    {
        if (!uniFiApi.IsAuthenticated) await AuthenticateTestMethod();

        // First retrieve all clients
        var task1 = await uniFiApi.GetAllClients();

        // Ensure we have at least one client on the UniFi network which we can temporarily rename to test the functionality
        if (task1.Count == 0)
        {
            Assert.Inconclusive("No clients currently on the UniFi network to use for testing to rename");
        }

        // Get the first client on the UniFi network so we can change its name
        var client = task1[0];

        // Ensure that we know the client its current name
        var currentName = client.FriendlyName;
        var temporaryName = string.Concat(currentName, " - Rename Test");

        // Rename the client
        var renameTask1 = await uniFiApi.RenameClient(client, temporaryName);

        // Retrieve the current state of all clients again
        var task2 = await uniFiApi.GetAllClients();

        // Filter to the same client as we renamed
        var clientAgain = task2.FirstOrDefault(c => c.MacAddress == client.MacAddress);
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
        var renameTask2 = await uniFiApi.RenameClient(client, currentName);

        // If the execution gets here it means the rename was successful
    }

    /// <summary>
    /// Tests if authorizing and unauthorizing of a guest on the UniFi network works
    /// </summary>
    [TestMethod]
    public async Task AuthorizeUnAuthorizeGuestTestMethod()
    {
        if (!uniFiApi.IsAuthenticated) await AuthenticateTestMethod();

        // First retrieve all active clients
        var task1 = await uniFiApi.GetActiveClients();

        // Ensure we have at least one client on the UniFi network which we can temporarily block to test the functionality
        if (task1.Count == 0)
        {
            Assert.Inconclusive("No clients currently on the UniFi network to use for testing to block");
        }

        // Get the first guest client on the UniFi network so we can change its authorized state
        var client = task1.FirstOrDefault(c => c.IsGuest.GetValueOrDefault(false));

        // Ensure that we have a guest client
        if (client == null)
        {
            Assert.Inconclusive($"Unable to find a guest on your network which is required for this test to work");
        }

        // Get the current authorized state of the guest
        bool guestInitiallyAuthorized = client.IsAuthorized.GetValueOrDefault(false);

        // Switch the authorized state of the guest
        var task2 = guestInitiallyAuthorized ? await uniFiApi.UnauthorizeGuest(client.MacAddress) : await uniFiApi.AuthorizeGuest(client.MacAddress);

        // Retrieve the active clients again
        var task3 = await uniFiApi.GetActiveClients();

        // Filter to the same client as we switched the authorization state of
        var clientAgain = task3.FirstOrDefault(c => c.MacAddress == client.MacAddress);
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
        var task4 = guestInitiallyAuthorized ? await uniFiApi.AuthorizeGuest(client.MacAddress) : await uniFiApi.UnauthorizeGuest(client.MacAddress);

        // If the execution gets here it means the block switching was successful
    }

    /// <summary>
    /// Tests retrieving all networks registered in UniFi
    /// </summary>
    [TestMethod]
    public async Task GetNetworksTestMethod()
    {
        if (!uniFiApi.IsAuthenticated) await AuthenticateTestMethod();

        var task = await uniFiApi.GetNetworks();
        Assert.IsTrue(task.Count > 0, "No networks found");
    }

    /// <summary>
    /// Tests retrieving all wireless networks registered in UniFi
    /// </summary>
    [TestMethod]
    public async Task GetWirelessNetworksTestMethod()
    {
        if (!uniFiApi.IsAuthenticated) await AuthenticateTestMethod();

        var task = await uniFiApi.GetWirelessNetworks();
        Assert.IsTrue(task.Count > 0, "No wireless networks found");
    }

    /// <summary>
    /// Tests if forcing a client to reconnect on the UniFi network works
    /// </summary>
    [TestMethod]
    public async Task ReconnectClientTestMethod()
    {
        if (!uniFiApi.IsAuthenticated) await AuthenticateTestMethod();

        // First retrieve all clients
        var task1 = await uniFiApi.GetAllClients();

        // Ensure we have at least one client on the UniFi network which we can force to reconnect
        if (task1.Count == 0)
        {
            Assert.Inconclusive("No clients currently on the UniFi network to use for testing to reconnect");
        }

        // Get the first client on the UniFi network so we can force to reconnect
        var client = task1[0];

        // Force the client to reconnect
        var reconnectTask = await uniFiApi.ReconnectClient(client);

        // If the execution gets here it means the reconnect was successful
    }
}
