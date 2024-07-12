namespace ChatApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<Chat> Chats { get; set; }
        public ICollection<ChatUsers> ChatUsers { get; set; }
    }
}
