using ChatApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Data
{
    public class ChatAppContext : DbContext
    {
        public ChatAppContext(DbContextOptions<ChatAppContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatUsers> ChatUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
               .HasKey(m => m.Id);
            modelBuilder.Entity<User>()
               .Property(m => m.Id).UseIdentityColumn();
            modelBuilder.Entity<User>()
              .Property(m => m.Name).IsRequired();



            modelBuilder.Entity<Message>()
                .HasKey(m => m.Id);
            modelBuilder.Entity<Message>()
              .Property(m => m.Id).UseIdentityColumn();
            modelBuilder.Entity<Message>()
                           .Property(m => m.Content)
                           .IsRequired();
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Message>()
                .HasOne(m => m.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Chat>()
               .HasKey(m => m.Id);
            modelBuilder.Entity<Chat>()
              .Property(m => m.Id).UseIdentityColumn();
            modelBuilder.Entity<Chat>()
                          .Property(m => m.Name)
                          .IsRequired();
            modelBuilder.Entity<Chat>()
               .HasOne(m => m.Creator)
               .WithMany(c => c.Chats)
               .HasForeignKey(m => m.CreatorId)
               .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<ChatUsers>()
           .HasKey(cu => new { cu.ChatId, cu.UserId });

            modelBuilder.Entity<ChatUsers>()
                .HasOne(cu => cu.Chat)
                .WithMany(c => c.ChatUsers)
                .HasForeignKey(cu => cu.ChatId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ChatUsers>()
                .HasOne(cu => cu.User)
                .WithMany(u => u.ChatUsers)
                .HasForeignKey(cu => cu.UserId)
                .OnDelete(DeleteBehavior.NoAction);


            base.OnModelCreating(modelBuilder);
        }

    }
}
