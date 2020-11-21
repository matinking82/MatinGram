using MatinGram.Application.Interfaces;
using MatinGram.Domain.Entities.Chatrooms;
using MatinGram.Domain.Entities.Messages;
using MatinGram.Domain.Entities.Relations;
using MatinGram.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Persistace.Context
{
    public class DataBaseContext : DbContext, IDataBaseContext
    {

        public DataBaseContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserImage> UserImages { get; set; }

        public DbSet<Chatroom> Chatrooms { get; set; }
        public DbSet<ChatroomImage> ChatroomImages { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<AdminInChatroom> AdminInChatrooms { get; set; }
        public DbSet<UserInChatroom> UserInChatrooms { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetRelations(modelBuilder);





            //Seed Data
            SeedData(modelBuilder);

            modelBuilder.Entity<User>().HasIndex(u => u.MobileNumber).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();

        }

        private static void SetRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chatroom>()
                .HasOne(g => g.Creator)
                .WithMany(u => u.CreatedChatrooms)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserImage>()
                .HasOne(i => i.User)
                .WithMany(u => u.UserImages)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ChatroomImage>()
                .HasOne(i => i.Chatroom)
                .WithMany(c => c.ChatroomImages)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Chatroom)
                .WithMany(c => c.Messages)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.Messages)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AdminInChatroom>()
                .HasOne(a => a.Chatroom)
                .WithMany(c => c.AdminInChatroom)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AdminInChatroom>()
                .HasOne(a => a.User)
                .WithMany(u => u.AdminInChatroom)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserInChatroom>()
                .HasOne(u => u.Chatroom)
                .WithMany(c => c.UserInChatrooms)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserInChatroom>()
                .HasOne(u => u.User)
                .WithMany(u => u.UserInChatrooms)
                .OnDelete(DeleteBehavior.NoAction);

        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            //Add Data Here
        }

    }
}
