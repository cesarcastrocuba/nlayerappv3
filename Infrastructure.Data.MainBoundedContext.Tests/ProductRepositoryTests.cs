namespace NLayerApp.Infrastructure.Data.MainBoundedContext.Tests
{
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
    using NLayerApp.Infrastructure.Data.MainBoundedContext.ERPModule.Repositories;
    using System;
    using System.Linq;
    using Xunit;

    [Collection("Our Test Collection #1")]
    public class ProductRepositoryTests : IClassFixture<MainBCUnitOfWorkInitializer>
    {
        protected MainBCUnitOfWorkInitializer fixture;
        public ProductRepositoryTests(MainBCUnitOfWorkInitializer fixture
            )
        {
            this.fixture = fixture;
        }

        [Fact]
        public void ProductRepositoryGetMethodReturnMaterializedEntityById()
        {
            //Arrange
            IProductRepository productRepository = new ProductRepository(fixture.unitOfWork, fixture.productLogger);

            var productId = new Guid("44668EBF-7B54-4431-8D61-C1298DB50857");
            Product product = null;

            //Act
            product = productRepository.Get(productId);

            //Assert
            Assert.NotNull(product);
            Assert.True(product.Id == productId);
        }

        [Fact]
        public void ProductRepositoryGetMethodReturnNullWhenIdIsEmpty()
        {
            //Arrange
            var productRepository = new ProductRepository(fixture.unitOfWork, fixture.productLogger);

            Product product = null;

            //Act
            product = productRepository.Get(Guid.Empty);

            //Assert
            Assert.Null(product);
        }

        [Fact]
        public void ProductRepositoryAddNewItemSaveItem()
        {
            //Arrange
            IProductRepository productRepository = new ProductRepository(fixture.unitOfWork, fixture.productLogger);

            var book = new Book("The book title", "Any book description", "Krasis Press", "ABC");

            book.ChangeUnitPrice(40);
            book.IncrementStock(1);
            book.GenerateNewIdentity();

            //Act
            productRepository.Add(book);
            fixture.unitOfWork.Commit();
          
        }

        [Fact]
        public void ProductRepositoryGetAllReturnMaterializedAllItems()
        {
            //Arrange
            IProductRepository productRepository = new ProductRepository(fixture.unitOfWork, fixture.productLogger);

            //Act
            var allItems = productRepository.GetAll();

            //Assert
            Assert.NotNull(allItems);
            Assert.True(allItems.Any());
        }

        [Fact]
        public void ProductRepositoryAllMatchingMethodReturnEntitiesWithSatisfiedCriteria()
        {
            //Arrange
            IProductRepository productRepository = new ProductRepository(fixture.unitOfWork, fixture.productLogger);

            var spec = ProductSpecifications.ProductFullText("book");

            //Act
            var result = productRepository.AllMatching(spec);

            //Assert
            Assert.NotNull(result.All(p => p.Title.Contains("book") || p.Description.Contains("book")));

        }

        [Fact]
        public void ProductRepositoryFilterMethodReturnEntitisWithSatisfiedFilter()
        {
            //Arrange
            IProductRepository productRepository = new ProductRepository(fixture.unitOfWork, fixture.productLogger);

            //Act
            var result = productRepository.GetFiltered(p => p.AmountInStock > 1);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.All(p=>p.AmountInStock > 1));
        }

        [Fact]
        public void ProductRepositoryPagedMethodReturnEntitiesInPageFashion()
        {
            //Arrange
            IProductRepository productRepository = new ProductRepository(fixture.unitOfWork, fixture.productLogger);

            //Act
            var pageI = productRepository.GetPaged(0, 1, b => b.Id, false);
            var pageII = productRepository.GetPaged(1, 1, b => b.Id, false);

            //Assert
            Assert.NotNull(pageI);
            Assert.True(pageI.Count() == 1);

            Assert.NotNull(pageII);
            Assert.True(pageII.Count() == 1);

            Assert.False(pageI.Intersect(pageII).Any());
        }
        [Fact]
        public void OrderRepositoryRemoveItemDeleteIt()
        {
            //Arrange
            IProductRepository productRepository = new ProductRepository(fixture.unitOfWork, fixture.productLogger);

            var book = new Book("The book title", "Any book description", "Krasis Press", "ABC");

            book.ChangeUnitPrice(40M);
            book.IncrementStock(1);
            book.GenerateNewIdentity();

            //Act
            productRepository.Add(book);
            fixture.unitOfWork.Commit();
            
        }
    }
}
