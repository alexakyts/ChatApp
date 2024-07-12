using ChatApp.Data;
using ChatApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Services;

public class ChatService : IChatService
{
    private readonly ChatAppContext _context;

    public ChatService(ChatAppContext context)
    {
        _context = context;
    }
    public async Task Connect(int userId, int chatId)
    {
        var chat = await _context.Chats.FirstOrDefaultAsync(c => c.Id == chatId);
        if (chat is null)
        {
            throw new Exception("Chat not found");
        }

        var chatUser = new ChatUsers()
        {
           ChatId = chat.Id,
           UserId = userId
        };

        await _context.ChatUsers.AddAsync(chatUser);
        await _context.SaveChangesAsync();
    }

    public async Task Disconnect(int userId, int chatId)
    {
        var chat = await _context.Chats.FirstOrDefaultAsync(c => c.Id == chatId);
        if (chat is null)
        {
            throw new Exception("Chat not found");
        }

        var chatUser = await _context.ChatUsers.FirstOrDefaultAsync(c => c.UserId == userId && c.ChatId == chat.Id);
        if (chatUser is null)
        {
            throw new Exception("User not found in that chat");
        }

        _context.ChatUsers.Remove(chatUser);
        await _context.SaveChangesAsync();
    }

    public async Task Send(int userId, int chatId, string content)
    {
        var chat = await _context.Chats.FirstOrDefaultAsync(c => c.Id == chatId);
        if (chat is null)
        {
            throw new Exception("Chat not found");
        }

        var message = new Message()
        {
            ChatId = chatId,
            UserId = userId,
            Content = content
        };

        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();
    }

    public async Task<int> Create(int userId, string name)
    {
        var chat = new Chat()
        {
            CreatorId = userId,
            Name = name
        };

        await _context.Chats.AddAsync(chat);
        await _context.SaveChangesAsync();

        return chat.Id;
    }

    public async Task Delete(int userId, int chatId)
    {
        var chat = await _context.Chats.FirstOrDefaultAsync(c => c.Id == chatId);
        if(chat is null)
        {
            throw new Exception("Chat not found");
        }

        if(chat.CreatorId != userId)
        {
            throw new Exception("Only chat creator can delete that chat");
        }

        _context.Chats.Remove(chat);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Chat>> Search(string name)
    {
        var chats = await _context.Chats.Where(c => c.Name.Contains(name)).ToListAsync();

        return chats;
    }
}
