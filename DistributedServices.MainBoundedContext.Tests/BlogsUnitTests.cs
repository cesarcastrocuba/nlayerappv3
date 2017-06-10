namespace NLayerApp.DistributedServices.MainBoundedContext.Tests
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Moq;
    using NLayerApp.Application.MainBoundedContext.BlogModule.Services;
    using NLayerApp.Domain.MainBoundedContext.BlogModule.Aggregates.BlogAgg;
    using NLayerApp.Infrastructure.Data.MainBoundedContext.BlogModule.Repositories;
    using NLayerApp.Infrastructure.Data.MainBoundedContext.UnitOfWork;
    using NLayerApp.Infrastructure.Data.Seedwork;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;
    public class BlogsUnitTests : TestsInitialize
    {
        private IBlogsService _blogsService;
        public BlogsUnitTests()
        {
            IQueryable<Blog> _fakeBlogs = new List<Blog>() {
             new Blog() { BlogId = 1, Name = "Blog 1", Url = "Url 1", Rating = 1 },
             new Blog() { BlogId = 2, Name = "Blog 2", Url = "Url 2", Rating = 2 },
             new Blog() { BlogId = 3, Name = "Blog 3", Url = "Url 3", Rating = 3 }
            }.AsQueryable();

            Mock<DbSet<Blog>> _mockSet = new Mock<DbSet<Blog>>();

            _mockSet.As<IAsyncEnumerable<Blog>>().Setup(m => m.GetEnumerator()).Returns(new TestAsyncEnumerator<Blog>(_fakeBlogs.GetEnumerator()));

            _mockSet.As<IQueryable<Blog>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<Blog>(_fakeBlogs.Provider));
            _mockSet.As<IQueryable<Blog>>().Setup(m => m.Expression).Returns(_fakeBlogs.Expression);
            _mockSet.As<IQueryable<Blog>>().Setup(m => m.ElementType).Returns(_fakeBlogs.ElementType);
            _mockSet.As<IQueryable<Blog>>().Setup(m => m.GetEnumerator()).Returns(_fakeBlogs.GetEnumerator());

            Mock<BloggingContext> _mockContext = new Mock<BloggingContext>();
            _mockContext.Setup(c => c.CreateSet<Blog>()).Returns(_mockSet.Object);
            _mockContext.Setup(c => c.Blogs).Returns(_mockSet.Object);

            Mock<ILogger<Repository<Blog>>> _mockLogger = new Mock<ILogger<Repository<Blog>>>();

            IBlogRepository _blogRepository = new BlogRepository(_mockContext.Object, _mockLogger.Object);

            _blogsService = new BlogsService(_blogRepository, null);
        }
        [Fact]
        public async Task FindBlogsAsync()
        {
            var blogs = await _blogsService.GetAllDTOAsync();

            Assert.NotNull(blogs);
            Assert.Equal(3, blogs.Count);
            Assert.Equal("Blog 1", blogs[0].Name);
            Assert.Equal("Blog 2", blogs[1].Name);
            Assert.Equal("Blog 3", blogs[2].Name);

        }


    }
}
