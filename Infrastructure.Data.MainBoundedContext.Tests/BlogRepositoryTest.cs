namespace NLayerApp.Infrastructure.Data.MainBoundedContext.Tests
{
    using NLayerApp.Infrastructure.Data.MainBoundedContext.BlogModule.Repositories;
    using NLayerApp.Infrastructure.Data.MainBoundedContext.UnitOfWork;
    using NLayerApp.Domain.MainBoundedContext.BlogModule.Aggregates.BlogAgg;
    using Xunit;
    using Microsoft.Extensions.Logging;
    using NLayerApp.Infrastructure.Data.Seedwork;
    public class BlogRepositoryTest
    {
        ILoggerFactory loggerFactory;
        ILogger<Repository<Blog>> logger;
        public BlogRepositoryTest()
        {
            loggerFactory = new LoggerFactory();
            logger = loggerFactory.CreateLogger<Repository<Blog>>();
        }
        [Fact]
        public void BlogRepositoryAddBlog()
        {
            //Arrange
            var unitOfWork = new BloggingContext();
            var blogRepository = new BlogRepository(unitOfWork, logger);

            var blogId = 1;

            var blog = new Blog()
            {
                BlogId = blogId,
                Name = "Name",
                Rating = 0,
                Url = "Url"
            };

            //Act
            blogRepository.Add(blog);
            unitOfWork.Commit();

            blog = blogRepository.Get(blogId);

            //Assert
            Assert.NotNull(blog);
            Assert.True(blog.BlogId == blogId);
        }
    }
}

