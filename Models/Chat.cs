namespace ChatApp.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CreatorId { get; set; }
        public virtual User Creator { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<ChatUsers> ChatUsers { get; set; }
    }
}
