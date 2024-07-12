using ChatApp.Models;

namespace ChatApp.Services;

public interface IUserService
{
    Task<int> Create(string name);
}
