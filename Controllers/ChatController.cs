using ChatApp.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost("create")]
    public async Task<ActionResult<int>> Create([FromBody] CreateChatDto createChatDto)
    {
        var chatId = await _chatService.Create(createChatDto.UserId, createChatDto.Name);
        return Ok(chatId);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(int userId, int chatId)
    {
        await _chatService.Delete(userId, chatId);
        return Ok();
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<ChatResponse>>> Search(string name)
    {
        var chats = await _chatService.Search(name);
        return Ok(chats.Select(c=>new ChatResponse()
        {
            Id = c.Id,
            Name = c.Name
        }).ToList());
    }
}
