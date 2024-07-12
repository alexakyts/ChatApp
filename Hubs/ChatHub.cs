//using Microsoft.AspNetCore.SignalR;

//namespace SignalRChat.Hubs
//{
//    public class ChatHub : Hub
//    {
//        public async Task SendMessage(string user, string message)
//        {
//            await Clients.All.SendAsync("ReceiveMessage", user, message);
//        }
//    }
//}
using ChatApp.Data;
using ChatApp.Models;
using ChatApp.Services;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs
{
    public class ChatHub(IChatService chat) : Hub
    {
        private readonly IChatService _chat = chat;

        public async Task JoinChat(int userId, int chatId)
        {
            await _chat.Connect(userId, chatId);
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }

        public async Task LeaveChat(int userId, int chatId)
        {
            await _chat.Disconnect(userId, chatId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
        }

        public async Task SendMessage(int userId, int chatId, string message)
        {
            await _chat.Send(userId, chatId, message);
            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", userId, message);
        }



    }
}