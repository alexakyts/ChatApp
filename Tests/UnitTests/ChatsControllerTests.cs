using ChatApp.Models;
using Microsoft.EntityFrameworkCore;
using ChatApp.Controllers;
using ChatApp.Data;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using System.Threading.Tasks;

namespace ChatApp.Tests.UnitTests
{
    public class ChatControllerTests
    {
        private DbContextOptions<ChatAppContext> _options;
        private Mock<IHubContext<ChatHub>> _mockHubContext;

        public ChatControllerTests()
        {
            _options = new DbContextOptionsBuilder<ChatAppContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _mockHubContext = new Mock<IHubContext<ChatHub>>();
        }

        [Fact]
        public async Task CreateChat_ShouldCreateNewChat()
        {
            // Arrange
            using (var context = new ChatAppContext(_options))
            {
                var controller = new ChatController(context, _mockHubContext.Object);
                var newChat = new Chat { Name = "Test Chat", CreatorId = 1 };

                // Act
                var result = await controller.CreateChat(newChat);

                // Assert
                var actionResult = Assert.IsType<ActionResult<Chat>>(result);
                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
                var returnValue = Assert.IsType<Chat>(createdAtActionResult.Value);

                Assert.NotNull(returnValue);
                Assert.Equal("Test Chat", returnValue.Name);
            }
        }

        [Fact]
        public async Task DeleteChat_ShouldDeleteExistingChat()
        {
            // Arrange
            using (var context = new ChatAppContext(_options))
            {
                var chat = new Chat { Name = "Test Chat", CreatorId = 1 };
                context.Chats.Add(chat);
                context.SaveChanges();

                var controller = new ChatController(context, _mockHubContext.Object);

                // Act
                var result = await controller.DeleteChat(chat.Id);

                // Assert
                Assert.IsType<NoContentResult>(result);
                Assert.Null(context.Chats.Find(chat.Id));
            }
        }
    }
}
