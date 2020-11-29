﻿// <auto-generated />
using System;
using MatinGram.Persistace.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MatinGram.Persistace.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    partial class DataBaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("MatinGram.Domain.Entities.Chatrooms.Chatroom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("ChatroomType")
                        .HasColumnType("int");

                    b.Property<long?>("CreatorId")
                        .HasColumnType("bigint");

                    b.Property<Guid>("Guid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("InsertTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("JoinLinkGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Chatrooms");
                });

            modelBuilder.Entity("MatinGram.Domain.Entities.Chatrooms.ChatroomImage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<int>("ChatroomId")
                        .HasColumnType("int");

                    b.Property<string>("ImageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("InsertTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ChatroomId");

                    b.ToTable("ChatroomImages");
                });

            modelBuilder.Entity("MatinGram.Domain.Entities.Messages.Message", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<int>("ChatroomID")
                        .HasColumnType("int");

                    b.Property<string>("ImageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SendDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("SenderId")
                        .HasColumnType("bigint");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ChatroomID");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("MatinGram.Domain.Entities.Relations.AdminInChatroom", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<int>("ChatroomId")
                        .HasColumnType("int");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ChatroomId");

                    b.HasIndex("UserId");

                    b.ToTable("AdminInChatrooms");
                });

            modelBuilder.Entity("MatinGram.Domain.Entities.Relations.UserInChatroom", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<int>("ChatroomId")
                        .HasColumnType("int");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ChatroomId");

                    b.HasIndex("UserId");

                    b.ToTable("UserInChatrooms");
                });

            modelBuilder.Entity("MatinGram.Domain.Entities.Users.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("Bio")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("HashKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("InsertTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastOnline")
                        .HasColumnType("datetime2");

                    b.Property<string>("MobileNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserInRole")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("MobileNumber")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasFilter("[Username] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MatinGram.Domain.Entities.Users.UserImage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("ImageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("InsertTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserImages");
                });

            modelBuilder.Entity("MatinGram.Domain.Entities.Chatrooms.Chatroom", b =>
                {
                    b.HasOne("MatinGram.Domain.Entities.Users.User", "Creator")
                        .WithMany("CreatedChatrooms")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("MatinGram.Domain.Entities.Chatrooms.ChatroomImage", b =>
                {
                    b.HasOne("MatinGram.Domain.Entities.Chatrooms.Chatroom", "Chatroom")
                        .WithMany("ChatroomImages")
                        .HasForeignKey("ChatroomId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Chatroom");
                });

            modelBuilder.Entity("MatinGram.Domain.Entities.Messages.Message", b =>
                {
                    b.HasOne("MatinGram.Domain.Entities.Chatrooms.Chatroom", "Chatroom")
                        .WithMany("Messages")
                        .HasForeignKey("ChatroomID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MatinGram.Domain.Entities.Users.User", "Sender")
                        .WithMany("Messages")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Chatroom");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("MatinGram.Domain.Entities.Relations.AdminInChatroom", b =>
                {
                    b.HasOne("MatinGram.Domain.Entities.Chatrooms.Chatroom", "Chatroom")
                        .WithMany("AdminInChatroom")
                        .HasForeignKey("ChatroomId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MatinGram.Domain.Entities.Users.User", "User")
                        .WithMany("AdminInChatroom")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Chatroom");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MatinGram.Domain.Entities.Relations.UserInChatroom", b =>
                {
                    b.HasOne("MatinGram.Domain.Entities.Chatrooms.Chatroom", "Chatroom")
                        .WithMany("UserInChatrooms")
                        .HasForeignKey("ChatroomId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MatinGram.Domain.Entities.Users.User", "User")
                        .WithMany("UserInChatrooms")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Chatroom");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MatinGram.Domain.Entities.Users.UserImage", b =>
                {
                    b.HasOne("MatinGram.Domain.Entities.Users.User", "User")
                        .WithMany("UserImages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MatinGram.Domain.Entities.Chatrooms.Chatroom", b =>
                {
                    b.Navigation("AdminInChatroom");

                    b.Navigation("ChatroomImages");

                    b.Navigation("Messages");

                    b.Navigation("UserInChatrooms");
                });

            modelBuilder.Entity("MatinGram.Domain.Entities.Users.User", b =>
                {
                    b.Navigation("AdminInChatroom");

                    b.Navigation("CreatedChatrooms");

                    b.Navigation("Messages");

                    b.Navigation("UserImages");

                    b.Navigation("UserInChatrooms");
                });
#pragma warning restore 612, 618
        }
    }
}
