namespace NLayerApp.Domain.MainBoundedContext.Tests
{
    using NLayerApp.Domain.MainBoundedContext.Aggregates.PostAgg;
    using NLayerApp.Domain.Seedwork.Specification;
    using Xunit;
    public class PostSpecificationsTests
    {
        [Fact]
        public void PostSpecificationNullTitleReturnTrueSpec()
        {
            //Arrange
            ISpecification<Post> spec = null;

            //Act
            spec = PostSpecifications.PostsContainsTitleOrContent(null);

            //assert
            Assert.IsType<TrueSpecification<Post>>(spec);

        }

        [Fact]
        public void PostSpecificationValidTitleReturnDirectSpec()
        {
            //Arrange
            ISpecification<Post> spec = null;

            //Act
            spec = PostSpecifications.PostsContainsTitleOrContent("Title");

            //assert
            Assert.IsType<AndSpecification<Post>>(spec);

        }
    }
}
