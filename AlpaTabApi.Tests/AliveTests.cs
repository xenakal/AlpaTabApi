using Microsoft.AspNetCore.Mvc.Testing;

namespace AlpaTabApi.Tests
{
    public class AliveTests
    {
        [Fact]
        public async Task TestRootEndpointAsync()
        {
            HttpClient client = HttpClientHelper.CreateClient();
            var response = await client.GetStringAsync("/");
            Assert.Equal(Helpers.Constants.WELCOME_API, response);
        }
    }
}