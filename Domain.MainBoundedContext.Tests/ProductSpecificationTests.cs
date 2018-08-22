using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
using NLayerApp.Domain.Seedwork.Specification;
using Xunit;

namespace Domain.MainBoundedContext.Tests
{
    public class ProductSpecificationTests
    {
        [Fact]
        public void ProductFullTextSpecificationEmptyDataReturnTrueSpecification()
        {
            //Arrange
            string productData = string.Empty;

            //Act
            var specification = ProductSpecifications.ProductFullText(productData);

            //Assert
            Assert.NotNull(specification);
            Assert.IsType<TrueSpecification<Product>>(specification);
        }
        [Fact]
        public void ProductFullTextSpecificationNullDataReturnTrueSpecification()
        {
            //Arrange
            string productData = null;

            //Act
            var specification = ProductSpecifications.ProductFullText(productData);

            //Assert
            Assert.NotNull(specification);
            Assert.IsType<TrueSpecification<Product>>(specification);
        }
        [Fact]
        public void ProductFullTextSpecificationNonEmptyDataReturnAndSpecification()
        {
            //Arrange
            string productData = "the product title or product description data";

            //Act
            var specification = ProductSpecifications.ProductFullText(productData);

            //Assert
            Assert.NotNull(specification);
            Assert.IsType<AndSpecification<Product>>(specification);
        }
    }
}
