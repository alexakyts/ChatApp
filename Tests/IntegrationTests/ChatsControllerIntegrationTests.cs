using ChatApp.Models;
using ChatApp;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;
using Xunit;
using ChatApp.Data;

namespace SimpleChatApp.Tests
{
    public class ChatsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public ChatsControllerIntegrationTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateChat_ReturnsSuccessStatusCode()
        {
            var client = _factory.CreateClient();

            var newChat = new Chat { Name = "Test Chat", CreatorId = 1 };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(newChat), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/chats", content);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task DeleteChat_WithoutPermission_ReturnsUnauthorized()
        {
            var options = new DbContextOptionsBuilder<ChatAppContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new ChatAppContext(options))
            {
                context.Chats.Add(new Chat { Id = 1, Name = "Test Chat", CreatorId = 2 });
                context.SaveChanges();
            }

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<ChatAppContext>(provider => new ChatAppContext(options));
                });
            }).CreateClient();

            var response = await client.DeleteAsync("/api/chats/1?userId=1");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
