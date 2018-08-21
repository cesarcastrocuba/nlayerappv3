using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using NLayerApp.Application.BlogBoundedContext.DTO;
using NLayerApp.Application.Seedwork;
using NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.BlogAgg;
using NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.PostAgg;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NLayerApp.DistributedServices.BlogBoundedContext.Tests
{
    [Collection("Our Test Collection #4")]
    /// <summary>
    /// Unit and integration tests for the /blogs API main route.
    /// </summary>
    public class BlogsIntegrationTests 
    {
        protected TestsInitialize fixture;
        public BlogsIntegrationTests(TestsInitialize fixture)
        {
            this.fixture = fixture;
        }
        private async Task<BlogDTO> GetBlogById(int id)
        {
            // Use the test server to make in-process API calls.

            var response = await fixture.server.CreateRequest(string.Format("/api/blogs/getbyid/{0}", id))
                .SendAsync("GET");

            // Assert 
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<BlogDTO>(content);

        }

        [Fact]
        public async Task GetBlogs_ShouldReturnListOfBlogs()
        {
            // Use the test server to make in-process API calls.

            var response = await fixture.server.CreateRequest("/api/blogs")
                .SendAsync("GET");

            // Assert 
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            var blogs = JsonConvert.DeserializeObject<List<BlogDTO>>(content);

            blogs.Should().NotBeNull();
            blogs.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetBlogById_ShouldReturnOneBlog()
        {
            var blog = await GetBlogById(1);
            blog.Should().NotBeNull();
        }

        [Fact]
        public async Task PostNullBlogs_ShouldReturnAValidationMessage()
        {
            // Use the test server to make in-process API calls.
            Blog blog = null;

            using (var client = fixture.server.CreateClient())
            {
                var blogData = new StringContent(JsonConvert.SerializeObject(blog.ProjectedAs<BlogDTO>()), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/blogs", blogData);

                //First Assert
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

                var content = await response.Content.ReadAsStringAsync();
                //Second Assert
                content.Should().Contain("A non-empty request body is required");
            }
        }

        [Fact]
        public async Task PostNonMandatoryFieldsBlogs_ShouldReturnAValidationMessage()
        {
            // Use the test server to make in-process API calls.
            Blog blog = new Blog()
            {
                Url = "Blog Test Url",
                Rating = 2
            };

            using (var client = fixture.server.CreateClient())
            {
                var blogData = new StringContent(JsonConvert.SerializeObject(blog.ProjectedAs<BlogDTO>()), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/blogs", blogData);

                //First Assert
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

                var content = await response.Content.ReadAsStringAsync();
                //Second Assert
                content.Should().Contain("Name is required");
            }
        }
        [Fact]
        public async Task PostBlogs_ShouldReturnABlogInserted()
        {
            // Use the test server to make in-process API calls.
            Blog blog = new Blog()
            {
                Name = "Blog Test Name",
                Url = "Blog Test Url",
                Rating = 2
            };

            using (var client = fixture.server.CreateClient())
            {
                var blogData = new StringContent(JsonConvert.SerializeObject(blog.ProjectedAs<BlogDTO>()), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/blogs", blogData);

                // Assert 
                response.EnsureSuccessStatusCode();

                //Second Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var content = await response.Content.ReadAsStringAsync();

                var blogInserted = JsonConvert.DeserializeObject<BlogDTO>(content);

                //Third Assert
                blogInserted.Should().NotBeNull();

                blogInserted.Name.Should().Be(blog.Name);
                blogInserted.Url.Should().Be(blog.Url);
                blogInserted.Rating.Should().Be(blog.Rating);

                var blogFinded = await GetBlogById(blogInserted.BlogId);

                //Fourth Assert
                blogFinded.Should().NotBeNull();

                blogFinded.Name.Should().Be(blog.Name);
                blogFinded.Url.Should().Be(blog.Url);
                blogFinded.Rating.Should().Be(blog.Rating);

            }
        }

        [Theory]
        [InlineData("awesome")]
        [InlineData("love")]
        [InlineData("core")]
        public async Task SearchPosts_ShouldReturnListOfPosts(string q)
        {
            // Use the test server to make in-process API calls.
            var response = await fixture.server.CreateRequest(string.Format("/api/posts?q={0}", q))
                .SendAsync("GET");

            // Assert 
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            var posts = JsonConvert.DeserializeObject<List<PostDTO>>(content);

            posts.Should().NotBeNull();
            posts.Count.Should().BeGreaterThan(0);
        }
    }
}
