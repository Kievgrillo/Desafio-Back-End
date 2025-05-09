using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;

namespace DigitalWallet.Tests.IntegrationsTests
{
    public class AuthTests : IClassFixture<WebApplicationFactory<Program>>
    {   
        private readonly WebApplicationFactory<Program> _Factory;

        public AuthTests(WebApplicationFactory<Program> factory)
        {
            _Factory = factory;
        }

        [Fact]
        public async Task Register_ReturnSuccessStatusCode()
        {
            var client = _Factory.CreateClient();
            var request = new
            {
                Username = "",
                Email = "",
                Password = ""
            };

            var response = await client.PostAsJsonAsync("/api/auth/register", request);
            response.EnsureSuccessStatusCode();
        }
    }
}
