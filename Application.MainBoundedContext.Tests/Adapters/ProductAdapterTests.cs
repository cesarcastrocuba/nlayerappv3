namespace NLayerApp.Application.MainBoundedContext.Tests
{
    using NLayerApp.Application.MainBoundedContext.DTO;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
    using NLayerApp.Infrastructure.Crosscutting.Adapter;
    using System.Collections.Generic;
    using Xunit;

    [Collection("Our Test Collection #2")]

    public class ProductAdapterTests 
    {
        protected TestsInitialize fixture;

        public ProductAdapterTests(TestsInitialize fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void ProductToProductDTOAdapter()
        {
            //Arrange
            var product = new Software("the title", "The description","AB001");
            product.ChangeUnitPrice(10);
            product.IncrementStock(10);
            product.GenerateNewIdentity();

            //Act
            ITypeAdapter adapter = TypeAdapterFactory.CreateAdapter();
            var productDTO = adapter.Adapt<Product, ProductDTO>(product);

            //Assert
            Assert.Equal(product.Id, productDTO.Id);
            Assert.Equal(product.Title, productDTO.Title);
            Assert.Equal(product.Description, productDTO.Description);
            Assert.Equal(product.AmountInStock, productDTO.AmountInStock);
            Assert.Equal(product.UnitPrice, productDTO.UnitPrice);
        }

        [Fact]
        public void EnumerableProductToListProductDTOAdapter()
        {
            //Arrange
            var software = new Software("the title", "The description","AB001");
            software.ChangeUnitPrice(10);
            software.IncrementStock(10);
            software.GenerateNewIdentity();

            var products = new List<Software>() { software };

            //Act
            ITypeAdapter adapter = TypeAdapterFactory.CreateAdapter();
            var productsDTO = adapter.Adapt<IEnumerable<Product>, List<ProductDTO>>(products);

            //Assert
            Assert.Equal(products[0].Id, productsDTO[0].Id);
            Assert.Equal(products[0].Title, productsDTO[0].Title);
            Assert.Equal(products[0].Description, productsDTO[0].Description);
            Assert.Equal(products[0].AmountInStock, productsDTO[0].AmountInStock);
            Assert.Equal(products[0].UnitPrice, productsDTO[0].UnitPrice);
        }

        [Fact]
        public void SoftwareToSoftwareDTOAdapter()
        {
            //Arrange
            var software = new Software("the title", "The description","AB001");
            software.ChangeUnitPrice(10);
            software.IncrementStock(10);
            software.GenerateNewIdentity();
            //Act
            ITypeAdapter adapter = TypeAdapterFactory.CreateAdapter();
            var softwareDTO = adapter.Adapt<Software, SoftwareDTO>(software);

            //Assert
            Assert.Equal(software.Id, softwareDTO.Id);
            Assert.Equal(software.Title, softwareDTO.Title);
            Assert.Equal(software.Description, softwareDTO.Description);
            Assert.Equal(software.AmountInStock, softwareDTO.AmountInStock);
            Assert.Equal(software.UnitPrice, softwareDTO.UnitPrice);
            Assert.Equal(software.LicenseCode, softwareDTO.LicenseCode);
        }

        [Fact]
        public void EnumerableSoftwareToListSoftwareDTOAdapter()
        {
            //Arrange
            var software = new Software("the title", "The description", "AB001");

            software.ChangeUnitPrice(10);
            software.IncrementStock(10);
            software.GenerateNewIdentity();

            var softwares = new List<Software>() { software };
            

            //Act
            ITypeAdapter adapter = TypeAdapterFactory.CreateAdapter();
            var softwaresDTO = adapter.Adapt<IEnumerable<Software>, List<SoftwareDTO>>(softwares);

            //Assert
            Assert.Equal(softwares[0].Id, softwaresDTO[0].Id);
            Assert.Equal(softwares[0].Title, softwaresDTO[0].Title);
            Assert.Equal(softwares[0].Description, softwaresDTO[0].Description);
            Assert.Equal(softwares[0].AmountInStock, softwaresDTO[0].AmountInStock);
            Assert.Equal(softwares[0].UnitPrice, softwaresDTO[0].UnitPrice);
            Assert.Equal(softwares[0].LicenseCode, softwaresDTO[0].LicenseCode);
        }

        [Fact]
        public void BookToBookDTOAdapter()
        {
            //Arrange
            var book = new Book("the title", "The description", "Krasis Press", "ABD12");

            book.ChangeUnitPrice(10);
            book.IncrementStock(10);

            book.GenerateNewIdentity();

            //Act
            ITypeAdapter adapter = TypeAdapterFactory.CreateAdapter();
            var bookDTO = adapter.Adapt<Book, BookDTO>(book);

            //Assert
            Assert.Equal(book.Id, bookDTO.Id);
            Assert.Equal(book.Title, bookDTO.Title);
            Assert.Equal(book.Description, bookDTO.Description);
            Assert.Equal(book.AmountInStock, bookDTO.AmountInStock);
            Assert.Equal(book.UnitPrice, bookDTO.UnitPrice);
            Assert.Equal(book.ISBN, bookDTO.ISBN);
            Assert.Equal(book.Publisher, bookDTO.Publisher);
        }

        [Fact]
        public void EnumerableBookToListBookDTOAdapter()
        {
            //Arrange
            var book = new Book("the title", "The description","Krasis Press","ABD12");

            book.ChangeUnitPrice(10);
            book.IncrementStock(10);
            book.GenerateNewIdentity();

            var books = new List<Book>() { book };

            //Act
            ITypeAdapter adapter = TypeAdapterFactory.CreateAdapter();
            var booksDTO = adapter.Adapt<IEnumerable<Book>, List<BookDTO>>(books);

            //Assert
            Assert.Equal(books[0].Id, booksDTO[0].Id);
            Assert.Equal(books[0].Title, booksDTO[0].Title);
            Assert.Equal(books[0].Description, booksDTO[0].Description);
            Assert.Equal(books[0].AmountInStock, booksDTO[0].AmountInStock);
            Assert.Equal(books[0].UnitPrice, booksDTO[0].UnitPrice);
            Assert.Equal(books[0].ISBN, booksDTO[0].ISBN);
            Assert.Equal(books[0].Publisher, booksDTO[0].Publisher);
        }
    }
}
