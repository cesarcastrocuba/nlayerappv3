using AutoMapper;
using NLayerApp.Infrastructure.Crosscutting.NetFramework.Adapter;
using NLayerApp.Infrastructure.Crosscutting.Tests.Classes;
using System.Collections.Generic;
using Xunit;

namespace NLayerApp.Infrastructure.Crosscutting.Tests
{
    public class AutomapperTypeAdapterTests : IClassFixture<AutomapperInitializer>
    {
        protected AutomapperInitializer fixture;

        public AutomapperTypeAdapterTests(AutomapperInitializer fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void AutoMapperTypeAdapterAdaptEntity()
        {
            var typeAdapter = new AutomapperTypeAdapter();

            var blog = new Blog()
            {
                 BlogId = 1,
                 Name = "Name",
                 Rating = 0,
                 Url = "Url"

            };

            //act

            var dto = typeAdapter.Adapt<Blog, BlogDTO>(blog);

            //Assert
            Assert.NotNull(dto);
            Assert.Equal(dto.BlogId, blog.BlogId);
            Assert.Equal(dto.Name, blog.Name);
            Assert.Equal(dto.Rating, blog.Rating);
            Assert.Equal(dto.Url, blog.Url);
        }

        [Fact]
        public void AutoMapperTypeAdapterAdaptEntityEnumerable()
        {
            var typeAdapter = new AutomapperTypeAdapter();

            var blog = new Blog()
            {
                 BlogId = 1,
                 Name = "Name",
                 Rating = 0,
                 Url = "Url"
            };

            //act

            var dto = typeAdapter.Adapt<IEnumerable<Blog>, List<BlogDTO>>(new Blog[] { blog});

            //Assert
            Assert.NotNull(dto);
            Assert.True(dto.Count == 1);
            
            Assert.Equal(dto[0].BlogId, blog.BlogId);
            Assert.Equal(dto[0].Name, blog.Name);
            Assert.Equal(dto[0].Rating, blog.Rating);
            Assert.Equal(dto[0].Url, blog.Url);
        }
    }
}
