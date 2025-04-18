﻿// <auto-generated />
using System;
using Learnify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Learnify.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250310193904_AddedMeetingsFunctionality")]
    partial class AddedMeetingsFunctionality
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.Connection", b =>
                {
                    b.Property<string>("ConnectionId")
                        .HasColumnType("text");

                    b.Property<string>("GroupName")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("ConnectionId");

                    b.HasIndex("GroupName");

                    b.HasIndex("UserId");

                    b.ToTable("Connections");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("PhotoId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("PrimaryLanguage")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("VideoId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PhotoId");

                    b.HasIndex("VideoId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.CourseRating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CourseId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<byte>("Rate")
                        .HasColumnType("smallint");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("CourseRatings");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.Group", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Name");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.MeetingConnection", b =>
                {
                    b.Property<string>("ConnectionId")
                        .HasColumnType("text");

                    b.Property<bool>("IsAuthor")
                        .HasColumnType("boolean");

                    b.Property<string>("SessionId")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("ConnectionId");

                    b.HasIndex("SessionId");

                    b.HasIndex("UserId");

                    b.ToTable("MeetingConnections");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.MeetingSession", b =>
                {
                    b.Property<string>("SessionId")
                        .HasColumnType("text");

                    b.Property<int>("CourseId")
                        .HasColumnType("integer");

                    b.HasKey("SessionId");

                    b.HasIndex("CourseId");

                    b.ToTable("MeetingSessions");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.MeetingStream", b =>
                {
                    b.Property<string>("StreamId")
                        .HasColumnType("text");

                    b.Property<string>("ConnectionId")
                        .HasColumnType("text");

                    b.HasKey("StreamId");

                    b.HasIndex("ConnectionId");

                    b.ToTable("MeetingStreams");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<string>("GroupId")
                        .HasColumnType("text");

                    b.Property<DateTime>("MessageSent")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("SenderId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.Paragraph", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CourseId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("isPublished")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("Paragraphs");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.PrivateFileData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("BlobName")
                        .HasColumnType("text");

                    b.Property<string>("ContainerName")
                        .HasColumnType("text");

                    b.Property<string>("ContentType")
                        .HasColumnType("text");

                    b.Property<int?>("CourseId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("FileDatas");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Expire")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("HasBeenUsed")
                        .HasColumnType("boolean");

                    b.Property<string>("Jwt")
                        .HasColumnType("text");

                    b.Property<string>("Refresh")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Jwt");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.Subtitle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Language")
                        .HasColumnType("integer");

                    b.Property<int?>("SubtitleFileId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("SubtitleFileId");

                    b.ToTable("Subtitles");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CardNumber")
                        .HasColumnType("text");

                    b.Property<string>("Company")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("ImageBlobName")
                        .HasColumnType("text");

                    b.Property<string>("ImageContainerName")
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<bool>("IsGoogleAuth")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("bytea");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<string>("Surname")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.UserBought", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("CourseId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "CourseId");

                    b.ToTable("UserBoughts");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.UserQuizAnswer", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<string>("LessonId")
                        .HasColumnType("text");

                    b.Property<string>("QuizId")
                        .HasColumnType("text");

                    b.Property<int>("AnswerIndex")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "LessonId", "QuizId");

                    b.HasIndex("UserId", "LessonId");

                    b.ToTable("UserQuizAnswers");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.Connection", b =>
                {
                    b.HasOne("Learnify.Core.Domain.Entities.Sql.Group", "Group")
                        .WithMany("Connections")
                        .HasForeignKey("GroupName");

                    b.HasOne("Learnify.Core.Domain.Entities.Sql.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.Course", b =>
                {
                    b.HasOne("Learnify.Core.Domain.Entities.Sql.PrivateFileData", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId");

                    b.HasOne("Learnify.Core.Domain.Entities.Sql.PrivateFileData", "Video")
                        .WithMany()
                        .HasForeignKey("VideoId");

                    b.Navigation("Photo");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.MeetingConnection", b =>
                {
                    b.HasOne("Learnify.Core.Domain.Entities.Sql.MeetingSession", "Session")
                        .WithMany()
                        .HasForeignKey("SessionId");

                    b.HasOne("Learnify.Core.Domain.Entities.Sql.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Session");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.MeetingSession", b =>
                {
                    b.HasOne("Learnify.Core.Domain.Entities.Sql.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.MeetingStream", b =>
                {
                    b.HasOne("Learnify.Core.Domain.Entities.Sql.MeetingConnection", "Connection")
                        .WithMany()
                        .HasForeignKey("ConnectionId");

                    b.Navigation("Connection");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.Message", b =>
                {
                    b.HasOne("Learnify.Core.Domain.Entities.Sql.Group", "Group")
                        .WithMany("Messages")
                        .HasForeignKey("GroupId");

                    b.HasOne("Learnify.Core.Domain.Entities.Sql.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId");

                    b.Navigation("Group");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.Paragraph", b =>
                {
                    b.HasOne("Learnify.Core.Domain.Entities.Sql.Course", "Course")
                        .WithMany("Paragraphs")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.Subtitle", b =>
                {
                    b.HasOne("Learnify.Core.Domain.Entities.Sql.PrivateFileData", "SubtitleFile")
                        .WithMany()
                        .HasForeignKey("SubtitleFileId");

                    b.Navigation("SubtitleFile");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.Course", b =>
                {
                    b.Navigation("Paragraphs");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.Group", b =>
                {
                    b.Navigation("Connections");

                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
