using ChatApp.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("create")]
    public async Task<ActionResult<int>> Create([FromBody] CreateUserDto createUserDto)
    {
        var chatId = await _userService.Create(createUserDto.Name);
        return Ok(chatId);
    }
}
