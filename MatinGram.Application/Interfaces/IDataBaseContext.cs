using MatinGram.Domain.Entities.Chatrooms;
using MatinGram.Domain.Entities.Messages;
using MatinGram.Domain.Entities.Relations;
using MatinGram.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MatinGram.Application.Interfaces
{
    public interface IDataBaseContext
    {

        DbSet<User> Users { get; set; }
        DbSet<UserImage> UserImages { get; set; }

        DbSet<Chatroom> Chatrooms { get; set; }
        DbSet<ChatroomImage> ChatroomImages { get; set; }

        DbSet<Message> Messages { get; set; }

        DbSet<AdminInChatroom> AdminInChatrooms { get; set; }
        DbSet<UserInChatroom> UserInChatrooms { get; set; }


        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,CancellationToken cancellationToken = new CancellationToken());
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());

    }
}
