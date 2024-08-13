using Microsoft.EntityFrameworkCore;
using DuongNDH2_PersonalDiaryAPI.Models;
using System;

namespace DuongNDH2_PersonalDiaryAPI.Models
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình User
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);  // Primary key configuration

            // Configure the Posts relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.User) 
                .HasForeignKey(p => p.UserId)  
                .IsRequired() 
                .OnDelete(DeleteBehavior.Cascade); 

            // Configure the Comments relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.Comments)  
                .WithOne(c => c.User) 
                .HasForeignKey(c => c.UserId) 
                .IsRequired()  
                .OnDelete(DeleteBehavior.Cascade);  

            // Configure the PostLikes relationship
            modelBuilder.Entity<User>().HasMany(u => u.PostLikes).WithOne(pl => pl.User).HasForeignKey(pl => pl.UserId).IsRequired();  

            modelBuilder.Entity<Post>()
                .HasKey(p => p.PostId);  

            // Configure the User relationship
            modelBuilder.Entity<Post>()
                .HasOne(p => p.User) 
                .WithMany(u => u.Posts)  
                .HasForeignKey(p => p.UserId)  
                .IsRequired()  
                .OnDelete(DeleteBehavior.Cascade);  

            // Configure the Comments relationship
            modelBuilder.Entity<Post>()
                .HasMany(p => p.Comments)  
                .WithOne(c => c.Post)  
                .HasForeignKey(c => c.PostId) 
                .IsRequired() 
                .OnDelete(DeleteBehavior.Cascade);  

            // Configure the Photos relationship
            modelBuilder.Entity<Post>()
                .HasMany(p => p.Photos) 
                .WithOne(ph => ph.Post)  
                .HasForeignKey(ph => ph.PostId) 
                .IsRequired() 
                .OnDelete(DeleteBehavior.Cascade); 

            // Configure the PostLikes relationship
            modelBuilder.Entity<Post>()
                .HasMany(p => p.PostLikes)  
                .WithOne(pl => pl.Post) 
                .HasForeignKey(pl => pl.PostId)  
                .IsRequired();  

            modelBuilder.Entity<Comment>()
    .HasKey(c => c.CommentId);  // Primary key configuration

            // Configure the relationship with Post
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)  
                .WithMany(p => p.Comments)  
                .HasForeignKey(c => c.PostId)  
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);  

            // Configure the relationship with User
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)  
                .WithMany(u => u.Comments) 
                .HasForeignKey(c => c.UserId) 
                .IsRequired()  
                .OnDelete(DeleteBehavior.Restrict);  

            // Configure PostLike entity
            modelBuilder.Entity<PostLike>()
                .HasKey(pl => pl.PostLikeId); 

            // Configure the relationship with Post
            modelBuilder.Entity<PostLike>()
                .HasOne(pl => pl.Post) 
                .WithMany(p => p.PostLikes)
                .HasForeignKey(pl => pl.PostId)  
                .IsRequired(); 

            // Configure the relationship with User
            modelBuilder.Entity<PostLike>()
                  .HasOne(pl => pl.User)
                  .WithMany(u => u.PostLikes) 
                  .HasForeignKey(pl => pl.UserId) 
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
