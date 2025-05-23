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
    [Migration("20250515194232_UpdatedUserQuizAnswerPrimaryKey")]
    partial class UpdatedUserQuizAnswerPrimaryKey
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("integer");

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

                    b.Property<int?>("VideoId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("PhotoId");

                    b.HasIndex("VideoId");

                    b.ToTable("Courses");
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

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.Paragraph", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CourseId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

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

                    b.HasKey("Id");

                    b.ToTable("FileDatas");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Expire")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("HasBeenUsed")
                        .HasColumnType("boolean");

                    b.Property<string>("Jwt")
                        .HasColumnType("text");

                    b.Property<string>("Refresh")
                        .HasColumnType("text");

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

                    b.Property<int>("Language")
                        .HasColumnType("integer");

                    b.Property<int?>("SubtitleFileId")
                        .HasColumnType("integer");

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

                    b.Property<string>("Email")
                        .HasColumnType("text");

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

                    b.HasIndex("CourseId");

                    b.ToTable("UserBoughts");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.UserQuizAnswer", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<string>("QuizId")
                        .HasColumnType("text");

                    b.Property<int>("AnswerIndex")
                        .HasColumnType("integer");

                    b.Property<string>("LessonId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "QuizId");

                    b.HasIndex("LessonId", "UserId");

                    b.ToTable("UserQuizAnswers");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.Course", b =>
                {
                    b.HasOne("Learnify.Core.Domain.Entities.Sql.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Learnify.Core.Domain.Entities.Sql.PrivateFileData", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId");

                    b.HasOne("Learnify.Core.Domain.Entities.Sql.PrivateFileData", "Video")
                        .WithMany()
                        .HasForeignKey("VideoId");

                    b.Navigation("Author");

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

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.UserBought", b =>
                {
                    b.HasOne("Learnify.Core.Domain.Entities.Sql.Course", "Course")
                        .WithMany("UserBoughts")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Learnify.Core.Domain.Entities.Sql.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Learnify.Core.Domain.Entities.Sql.Course", b =>
                {
                    b.Navigation("Paragraphs");

                    b.Navigation("UserBoughts");
                });
#pragma warning restore 612, 618
        }
    }
}
