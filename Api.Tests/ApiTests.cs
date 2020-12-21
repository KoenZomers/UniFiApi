using System;
using Xunit;
using KoenZomers.UniFi.Api.Tests.Fakes;
using System.Threading.Tasks;

namespace KoenZomers.UniFi.Api.Tests
{
    public class ApiTests
    {
        [Fact]
        public void Should_BuildApi_When_Requested()
        {
            IApi api = new Api(new Uri("https://test.com"));
        }

        [Fact]
        public async Task Should_Authenticate_When_Requested()
        {
            IApi api = new Api(
                new Uri("https://test.com"),
                "test",
                new FakeHttpUtility());

            Assert.True(await api.Authenticate("test", "test"));
            Assert.True(api.IsAuthenticated);
        }
    }
}
