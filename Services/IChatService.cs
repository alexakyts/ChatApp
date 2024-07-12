using ChatApp.Models;

namespace ChatApp.Services;

public interface IChatService
{
    Task Connect(int userId, int chatId);
    Task Send(int userId, int chatId, string content);
    Task Disconnect(int userId, int chatId);

    Task<int> Create(int userId, string name);
    Task Delete(int userId, int chatId);
    Task<List<Chat>> Search(string name);
}
