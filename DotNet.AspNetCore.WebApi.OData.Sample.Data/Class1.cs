using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.AspNetCore.WebApi.OData.Sample.Data
{
    public class BloggingContextFactory : IDesignTimeDbContextFactory<BloggingContext>
    {
        public BloggingContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BloggingContext>();
            var connection = @"Server=(localdb)\mssqllocaldb;Database=EFGetStarted.AspNetCore.NewDb;Trusted_Connection=True;ConnectRetryCount=0";

            optionsBuilder.UseLazyLoadingProxies(true);
            optionsBuilder.UseSqlServer(connection);

            return new BloggingContext(optionsBuilder.Options);
        }
    }

    public class BloggingContext : DbContext
    {
        public BloggingContext(DbContextOptions<BloggingContext> options)
            : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = true;
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            this.ChangeTracker.AutoDetectChangesEnabled = true;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connection = @"Server=(localdb)\mssqllocaldb;Database=EFGetStarted.AspNetCore.NewDb;Trusted_Connection=True;ConnectRetryCount=0";
            optionsBuilder.UseLazyLoadingProxies(true);
            optionsBuilder.UseSqlServer(connection);
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
    }

    public class Blog
    {
        public Blog()
        {
            this.Posts = new List<Post>();
        }

        public int BlogId { get; set; }
        public string Url { get; set; }

        public virtual List<Post> Posts { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
    }
}
