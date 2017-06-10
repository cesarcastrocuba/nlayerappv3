namespace NLayerApp.Application.MainBoundedContext.Tests.Adapters
{
    using NLayerApp.Application.MainBoundedContext.DTO;
    using NLayerApp.Domain.MainBoundedContext.BlogModule.Aggregates.BlogAgg;
    using NLayerApp.Infrastructure.Crosscutting.Adapter;
    using System.Collections.Generic;
    using Xunit;
    using System.Linq;
    public class BlogAdapterTests : TestsInitialize
    {
        [Fact]
        public void BlogToBlogDTOAdapter()
        {
            //Arrange
            var blog = BlogFactory.CreateBlog("Name", "Url", 0);

            //Act
            ITypeAdapter adapter = TypeAdapterFactory.CreateAdapter();
            var dto = adapter.Adapt<Blog, BlogDTO>(blog);

            //Assert
            Assert.Equal(blog.BlogId, dto.BlogId);
            Assert.Equal(blog.Name, dto.Name);
            Assert.Equal(blog.Url, dto.Url);
            Assert.Equal(blog.Rating, dto.Rating);
        }
        [Fact]
        public void BlogEnumerableToBlogDTOList()
        {
            //Arrange
            var blog = BlogFactory.CreateBlog("Name", "Url", 0);

            IEnumerable<Blog> blogs = new List<Blog>() { blog };

            //Act
            ITypeAdapter adapter = TypeAdapterFactory.CreateAdapter();
            var dtos = adapter.Adapt<IEnumerable<Blog>, List<BlogDTO>>(blogs);

            //Assert
            Assert.NotNull(dtos);
            Assert.True(dtos.Any());
            Assert.True(dtos.Count == 1);

            var dto = dtos[0];

            Assert.Equal(blog.BlogId, dto.BlogId);
            Assert.Equal(blog.Name, dto.Name);
            Assert.Equal(blog.Url, dto.Url);
            Assert.Equal(blog.Rating, dto.Rating);
        }
    }
}
