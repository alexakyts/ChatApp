using ChatApp.Data;
using ChatApp.Models;

namespace ChatApp.Services;

public class UserService : IUserService
{
    private readonly ChatAppContext _context;

    public UserService(ChatAppContext context)
    {
        _context = context;
    }
    

    public async Task<int> Create(string name)
    {
        var user = new User()
        {
            Name = name
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user.Id;
    }

   
}
