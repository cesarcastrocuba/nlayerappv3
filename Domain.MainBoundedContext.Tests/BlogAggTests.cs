namespace NLayerApp.Domain.MainBoundedContext.Tests
{
    using NLayerApp.Domain.MainBoundedContext.BlogModule.Aggregates.BlogAgg;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Xunit;

    public class BlogAggTests : TestsInitialize
    {
        [Fact]
        public void BlogWithEmptyNameValidationError()
        {
            //Arrange
            var blog = new Blog();
            blog.Rating = 0;
            blog.Url = "Url";

            //act
            var validationContext = new ValidationContext(blog, null, null);
            var validationResults = blog.Validate(validationContext);

            //assert
            Assert.NotNull(validationResults);
            Assert.True(validationResults.Any());
            Assert.True(validationResults.First().MemberNames.Contains("Name"));
        }

        [Fact]
        public void BlogWithEmptyUrlValidationError()
        {
            //Arrange
            var blog = new Blog();
            blog.Name = "Name";
            blog.Rating = 0;

            //act
            var validationContext = new ValidationContext(blog, null, null);
            var validationResults = blog.Validate(validationContext);

            //assert
            Assert.NotNull(validationResults);
            Assert.True(validationResults.Any());
            Assert.True(validationResults.First().MemberNames.Contains("Url"));
        }

        [Fact]
        public void BlogCountValidationError()
        {
            //Arrange
            var blog = new Blog();
            
            //act
            var validationContext = new ValidationContext(blog, null, null);
            var validationResults = blog.Validate(validationContext);

            //assert
            Assert.NotNull(validationResults);
            Assert.True(validationResults.Any());
            Assert.True(validationResults.Count() == 2);
        }
    }
}
