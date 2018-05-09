using NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.BlogAgg;
using NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.ImageAgg;
using NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.PostAgg;
using NLayerApp.Infrastructure.Data.BlogBoundedContext.UnitOfWork;
using System.Linq;

namespace NLayerApp.DistributedServices.BlogBoundedContext
{
    public static class DbInitializer
    {
        public static void Initialize(BloggingContext context)
        {
            context.Database.EnsureCreated();

            // Seed the database
            if (context.Blogs.Any()) return;
            var blog1 = new Blog() { Name = "API", Rating = 5, Url = "/blogs/api" };
            context.Blogs.Add(blog1);
            var blog2 = new Blog() { Name = "Angular", Rating = 5, Url = "/blogs/angular" };
            context.Blogs.Add(blog2);
            var blog3 = new Blog() { Name = ".NET Core", Rating = 5, Url = "/blogs/dotnet" };
            context.Blogs.Add(blog3);

            context.SaveChanges();

            var post1 = new Post() { Blog = blog1, Title = "Awesome API", Content = "APIs are awesome" };
            context.Posts.Add(post1);
            var post2 = new Post() { Blog = blog1, Title = "Love APIs", Content = "Let's build more APIs" };
            context.Posts.Add(post2);
            var post3 = new Post() { Blog = blog1, Title = ".NET Core WebApi", Content = "Can be hosted on Azure" };
            context.Posts.Add(post3);

            context.SaveChanges();

            var image1 = new Image() { Post = post1, Title = "Title of Image 1", Url = "Url of Image 1" };
            context.Images.Add(image1);
            var image2 = new Image() { Post = post1, Title = "Title of Image 2", Url = "Url of Image 2" };
            context.Images.Add(image2);
            var image3 = new Image() { Post = post1, Title = "Title of Image 3", Url = "Url of Image 3" };
            context.Images.Add(image3);

            context.SaveChanges();
        }
        
    }
}
