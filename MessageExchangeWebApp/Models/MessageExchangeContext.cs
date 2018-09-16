using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MessageExchangeWebApp.Models
{
    public class MessageExchangeContext : DbContext
    {
        public MessageExchangeContext() : base("DbConnection")
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>()
                .HasMany(p => p.Messages)
                .WithRequired(p => p.Chat)
                .HasForeignKey(s => s.ChatId)
                .WillCascadeOnDelete(true);
        }

    }
}