using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace KoenZomers.UniFi.Api.UnitTest
{
    [TestClass]
    public class UnitTest
    {
        private static Library.Api uniFiApi;

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            uniFiApi = new Library.Api(new Uri(ConfigurationManager.AppSettings["UniFiControllerUrl"]));
            uniFiApi.DisableSslValidation();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            uniFiApi.Dispose();
        }

        [TestMethod]
        public void AuthenticateTestMethod()
        {
            var task = uniFiApi.Authenticate(ConfigurationManager.AppSettings["UniFiControllerUsername"], ConfigurationManager.AppSettings["UniFiControllerPassword"]);
            task.Wait();
            Assert.IsTrue(task.Result, "Authentication failed");
        }

        [TestMethod]
        public void GetAccessPointsTestMethod()
        {
            if (!uniFiApi.IsAuthenticated) AuthenticateTestMethod();

            var task = uniFiApi.GetAccessPoints();
            task.Wait();
            Assert.IsTrue(task.Result.Count > 0, "No access points found");
        }

        [TestMethod]
        public void GetActiveClientsTestMethod()
        {
            if (!uniFiApi.IsAuthenticated) AuthenticateTestMethod();

            var task = uniFiApi.GetActiveClients();
            task.Wait();
            Assert.IsTrue(task.Result.Count > 0, "No active clients found");
        }
    }
}
