using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using OrderService.API;
using Xunit;

namespace OrderService.IntegrationTest
{
    public class HealthCheckTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public HealthCheckTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Should200()
        {
            HttpClient client = _factory.CreateClient();

            using HttpResponseMessage response = await client.GetAsync("/hc");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
