namespace NLayerApp.Domain.MainBoundedContext.Tests
{
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
    using System;
    using Xunit;

    public class ProductAggTests : TestsInitialize
    {
        [Fact]
        public void CannotCreateAProductWithNullTitle()
        {
            //Arrange and Act
            Exception ex = Assert.Throws<ArgumentNullException>(() => new Book(null, "the description", "publisher", "isbn"));
            Assert.IsType(typeof(ArgumentNullException), ex);
        }
        [Fact]
        public void CannotCreateAProductWithNullDescription()
        {
            //Arrange and Act
            Exception ex = Assert.Throws<ArgumentNullException>(() => new Book("the title", null, "publisher", "isbn"));
            Assert.IsType(typeof(ArgumentNullException), ex);
        }
        [Fact]
        public void CannotCreateAProductWithNullPublisher()
        {
            //Arrange and Act
            Exception ex = Assert.Throws<ArgumentNullException>(() => new Book("the title", "description", null, "isbn"));
            Assert.IsType(typeof(ArgumentNullException), ex);
        }
        [Fact]
        public void CannotCreateAProductWithNullIsbn()
        {
            //Arrange and Act
            Exception ex = Assert.Throws<ArgumentNullException>(() => new Book("the title", "description", "publisher", null));
            Assert.IsType(typeof(ArgumentNullException), ex);
        }
        [Fact]
        public void CannotCreateAProductWithEmptyTitle()
        {
            //Arrange and Act
            Exception ex = Assert.Throws<ArgumentNullException>(() => new Book(string.Empty, "the description", "publisher", "isbn"));
            Assert.IsType(typeof(ArgumentNullException), ex);
        }
        [Fact]
        public void CannotCreateAProductWithEmptyDescription()
        {
            //Arrange and Act
            Exception ex = Assert.Throws<ArgumentNullException>(() => new Book("the title", string.Empty, "publisher", "isbn"));
            Assert.IsType(typeof(ArgumentNullException), ex);
        }
        [Fact]
        public void CannotCreateAProductWithEmptyPublisher()
        {
            //Arrange and Act
            Exception ex = Assert.Throws<ArgumentNullException>(() => new Book("the title", "description", string.Empty, "isbn"));
            Assert.IsType(typeof(ArgumentNullException), ex);
        }
        [Fact]
        public void CannotCreateAProductWithEmptyIsbn()
        {
            //Arrange and Act
            Exception ex = Assert.Throws<ArgumentNullException>(() => new Book("the title", "description", "publisher", string.Empty));
            Assert.IsType(typeof(ArgumentNullException), ex);
        }
        [Fact]
        public void CannotCreateAProductWithWhitespaceTitle()
        {
            //Arrange and Act
            Exception ex = Assert.Throws<ArgumentNullException>(() => new Book(" ", "the description", "publisher", "isbn"));
            Assert.IsType(typeof(ArgumentNullException), ex);
        }
        [Fact]
        public void CannotCreateAProductWithWhitespaceDescription()
        {
            //Arrange and Act
            Exception ex = Assert.Throws<ArgumentNullException>(() => new Book("the title", " ", "publisher", "isbn"));
            Assert.IsType(typeof(ArgumentNullException), ex);
        }
        [Fact]
        public void CannotCreateAProductWithWhitespacePublisher()
        {
            //Arrange and Act
            Exception ex = Assert.Throws<ArgumentNullException>(() => new Book("the title", "description", " ", "isbn"));
            Assert.IsType(typeof(ArgumentNullException), ex);
        }
        [Fact]
        public void CannotCreateAProductWithWhitespaceISBN()
        {
            //Arrange and Act
            Exception ex = Assert.Throws<ArgumentNullException>(() => new Book("the title", "description", "publisher", " "));
            Assert.IsType(typeof(ArgumentNullException), ex);
        }
    }
}
