
namespace NLayerApp.Application.MainBoundedContext.Tests.Services
{
    using Microsoft.Extensions.Logging;
    using Moq;
    using NLayerApp.Application.MainBoundedContext.DTO;
    using NLayerApp.Application.MainBoundedContext.ERPModule.Services;
    using NLayerApp.Application.Seedwork;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
    using NLayerApp.Domain.Seedwork.Specification;
    using NLayerApp.Infrastructure.Data.MainBoundedContext.UnitOfWork;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Xunit;

    [Collection("Our Test Collection #2")]

    public class SalesAppServiceTests 
    {
        protected TestsInitialize fixture;

        public SalesAppServiceTests(TestsInitialize fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void FindOrdersInPageThrowArgumentExceptionWhenPageDataIsInvalid()
        {
            //Arrange
            var customerRepository = new Mock<ICustomerRepository>();
            var productRepository = new Mock<IProductRepository>();
            var orderRepository = new Mock<IOrderRepository>();
            Mock<ILogger<SalesAppService>> _mockLogger = new Mock<ILogger<SalesAppService>>();

            var salesManagement = new SalesAppService(productRepository.Object, orderRepository.Object, customerRepository.Object, _mockLogger.Object);
            
            Exception ex = Assert.Throws<ArgumentException>(() =>
            {
                //act
                var resultInvalidPageIndex = salesManagement.FindOrders(-1, 1);

                //Assert
                Assert.Null(resultInvalidPageIndex);
            }
            );

            Assert.IsType(typeof(ArgumentException), ex);
        }
        [Fact]
        public void FindOrdersWithInvalidPageIndexThrowException()
        {
            //Arrange

            var customerRepository = new Mock<ICustomerRepository>();
            var productRepository = new Mock<IProductRepository>();
            var orderRepository = new Mock<IOrderRepository>();
            Mock<ILogger<SalesAppService>> _mockLogger = new Mock<ILogger<SalesAppService>>();

            orderRepository
                .Setup(x => x.GetPaged<DateTime>(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Expression<Func<Order, DateTime>>>(), It.IsAny<bool>()))
                .Returns((Int32 index, Int32 count, Expression<Func<Order, DateTime>> order, bool ascending) => {
                    return new List<Order>();
                });

            var salesManagement = new SalesAppService(productRepository.Object, orderRepository.Object, customerRepository.Object, _mockLogger.Object);
            
            Exception ex = Assert.Throws<ArgumentException>(() =>
            {
                //act
                var result = salesManagement.FindOrders(-1, 1);
            }
            );

            Assert.IsType(typeof(ArgumentException), ex);

        }
        [Fact]
        public void FindOrdersInDateRangeReturnNullWhenNoData()
        {
            //Arrange

            var customerRepository = new Mock<ICustomerRepository>();
            var productRepository = new Mock<IProductRepository>();
            var orderRepository = new Mock<IOrderRepository>();
            Mock<ILogger<SalesAppService>> _mockLogger = new Mock<ILogger<SalesAppService>>();

            orderRepository
             .Setup(x => x.AllMatching(It.IsAny<ISpecification<Order>>()))
             .Returns((ISpecification<Order> spec) => {
                 return new List<Order>();
             });

            var salesManagement = new SalesAppService(productRepository.Object, orderRepository.Object, customerRepository.Object, _mockLogger.Object);

            //act
            var result = salesManagement.FindOrders(DateTime.Now.AddDays(-2), DateTime.Now.AddDays(+2));

            //Assert
            Assert.Null(result);
        }
        [Fact]
        public void FindOrderReturnNullIfCustomerIdIsEmpty()
        {
            //Arrange

            var customerRepository = new Mock<ICustomerRepository>();
            var productRepository = new Mock<IProductRepository>();
            var orderRepository = new Mock<IOrderRepository>();
            Mock<ILogger<SalesAppService>> _mockLogger = new Mock<ILogger<SalesAppService>>();
            
            orderRepository
             .Setup(x => x.GetFiltered(It.IsAny<Expression<Func<Order, bool>>>()))
             .Returns((Expression<Func<Order, bool>> expression) => {
                 return null;
             });

            var salesManagement = new SalesAppService(productRepository.Object, orderRepository.Object, customerRepository.Object, _mockLogger.Object);

            //act
            var result = salesManagement.FindOrders(Guid.Empty);


            //Assert
            Assert.Null(result);
        }
        [Fact]
        public void FindOrdersMaterializeResultsIfCustomerExist()
        {
            //Arrange
            var customerRepository = new Mock<ICustomerRepository>();
            var productRepository = new Mock<IProductRepository>();
            var orderRepository = new Mock<IOrderRepository>();
            Mock<ILogger<SalesAppService>> _mockLogger = new Mock<ILogger<SalesAppService>>();

            orderRepository
             .Setup(x => x.GetFiltered(It.IsAny<Expression<Func<Order, bool>>>()))
             .Returns((Expression<Func<Order, bool>> filter) => {
                 var orders = new List<Order>();
                 var customer = new Customer();
                 customer.ChangeCurrentIdentity(Guid.NewGuid());
                 orders.Add(OrderFactory.CreateOrder(customer, "name", "city", "address", "zipcode"));

                 return orders;
             });


            var salesManagement = new SalesAppService(productRepository.Object, orderRepository.Object, customerRepository.Object, _mockLogger.Object);

            //act
            var result = salesManagement.FindOrders(Guid.NewGuid());


            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 1);
        }

        [Fact]
        public void FindOrdersInPageMaterializeResults()
        {
            //Arrange
            
            var customerRepository = new Mock<ICustomerRepository>();
            var productRepository = new Mock<IProductRepository>();
            var orderRepository = new Mock<IOrderRepository>();
            Mock<ILogger<SalesAppService>> _mockLogger = new Mock<ILogger<SalesAppService>>();

            orderRepository
                .Setup(x => x.GetPaged<DateTime>(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Expression<Func<Order, DateTime>>>(), It.IsAny<bool>()))
                .Returns((Int32 index, Int32 count, Expression<Func<Order, DateTime>> order, bool ascending) => {
                    var item = new Order();
                    item.GenerateNewIdentity();
                    item.SetTheCustomerReferenceForThisOrder(Guid.NewGuid());

                    return new List<Order>()
                    {
                        item
                    };
                });

            var salesManagement = new SalesAppService(productRepository.Object, orderRepository.Object, customerRepository.Object, _mockLogger.Object);

            //act
            var result = salesManagement.FindOrders(0, 1);

            //Assert

            Assert.NotNull(result);
            Assert.True(result.Any());

        }
        [Fact]
        public void FindOrdersInDateRangeMaterializeResults()
        {
            //Arrange
            
            var customerRepository = new Mock<ICustomerRepository>();
            var productRepository = new Mock<IProductRepository>();
            var orderRepository = new Mock<IOrderRepository>();
            Mock<ILogger<SalesAppService>> _mockLogger = new Mock<ILogger<SalesAppService>>();

            orderRepository
             .Setup(x => x.AllMatching(It.IsAny<ISpecification<Order>>()))
             .Returns((ISpecification<Order> spec) => {
                 var order = new Order();
                 order.GenerateNewIdentity();
                 order.SetTheCustomerReferenceForThisOrder(Guid.NewGuid());

                 return new List<Order>()
                 {
                    order
                 };
             });


            var salesManagement = new SalesAppService(productRepository.Object, orderRepository.Object, customerRepository.Object, _mockLogger.Object);

            //act
            var result = salesManagement.FindOrders(DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1));

            //Assert

            Assert.NotNull(result);
            Assert.True(result.Any());

        }

        [Fact]
        public void FindProductsInPageThrowExceptionWhenPageIsInvalid()
        {
            //Arrange
            
            var customerRepository = new Mock<ICustomerRepository>();
            var productRepository = new Mock<IProductRepository>();
            var orderRepository = new Mock<IOrderRepository>();
            Mock<ILogger<SalesAppService>> _mockLogger = new Mock<ILogger<SalesAppService>>();

            var salesManagement = new SalesAppService(productRepository.Object, orderRepository.Object, customerRepository.Object, _mockLogger.Object);
            
            Exception ex = Assert.Throws<ArgumentException>(() =>
            {
                //act
                var resultInvalidPageIndex = salesManagement.FindProducts(-1, 1);
            }
            );

            Assert.IsType(typeof(ArgumentException), ex);
        }

        [Fact]
        public void FindProductsInPageReturnNullWhenNoData()
        {
            //Arrange
            
            var customerRepository = new Mock<ICustomerRepository>();
            var orderRepository = new Mock<IOrderRepository>();
            var productRepository = new Mock<IProductRepository>();
            Mock<ILogger<SalesAppService>> _mockLogger = new Mock<ILogger<SalesAppService>>();

            productRepository
                .Setup(x => x.GetPaged<string>(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Expression<Func<Product, string>>>(), It.IsAny<bool>()))
                .Returns((Int32 index, Int32 count, Expression<Func<Product, string>> order, bool ascending) => {
                    return new List<Product>();
                });

            var salesManagement = new SalesAppService(productRepository.Object, orderRepository.Object, customerRepository.Object, _mockLogger.Object);

            //act
            var result = salesManagement.FindProducts(0, 1);


            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void FindProductsInPageMaterializeResults()
        {
            //Arrange
            var customerRepository = new Mock<ICustomerRepository>();
            var productRepository = new Mock<IProductRepository>();
            var orderRepository = new Mock<IOrderRepository>();
            Mock<ILogger<SalesAppService>> _mockLogger = new Mock<ILogger<SalesAppService>>();

            productRepository
                .Setup(x => x.GetPaged<string>(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Expression<Func<Product, string>>>(), It.IsAny<bool>()))
                .Returns((Int32 index, Int32 count, Expression<Func<Product, string>> order, bool ascending) => {
                    var book = new Book("title", "description", "publisher", "isbn");
                    book.ChangeUnitPrice(10M);
                    book.GenerateNewIdentity();

                    var software = new Software("title", "description", "license code");
                    software.ChangeUnitPrice(10);
                    software.GenerateNewIdentity();

                    return new List<Product>()
                {
                    book,
                    software
                };

                });

            var salesManagement = new SalesAppService(productRepository.Object, orderRepository.Object, customerRepository.Object, _mockLogger.Object);

            //act
            var result = salesManagement.FindProducts(0, 2);


            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 2);
        }
        [Fact]
        public void FindProductsByFilterMaterializeResults()
        {
            //Arrange
            var customerRepository = new Mock<ICustomerRepository>();
            var productRepository = new Mock<IProductRepository>();
            var orderRepository = new Mock<IOrderRepository>();
            Mock<ILogger<SalesAppService>> _mockLogger = new Mock<ILogger<SalesAppService>>();

            productRepository
             .Setup(x => x.AllMatching(It.IsAny<ISpecification<Product>>()))
             .Returns((ISpecification<Product> spec) => {
                 var book = new Book("title", "description", "publisher", "isbn");
                 book.ChangeUnitPrice(10);
                 book.GenerateNewIdentity();

                 var software = new Software("title", "description", "license code");
                 software.ChangeUnitPrice(10);
                 software.GenerateNewIdentity();

                 return new List<Product>()
                    {
                        book,
                        software
                    };
             });

            var salesManagement = new SalesAppService(productRepository.Object, orderRepository.Object, customerRepository.Object, _mockLogger.Object);

            //act
            var result = salesManagement.FindProducts("filter text");

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 2);
        }
        [Fact]
        public void AddNewOrderWithoutCustomerIdThrowArgumentException()
        {
            //Arrange 
            
            var customerRepository = new Mock<ICustomerRepository>();
            var productRepository = new Mock<IProductRepository>();
            var orderRepository = new Mock<IOrderRepository>();
            Mock<ILogger<SalesAppService>> _mockLogger = new Mock<ILogger<SalesAppService>>();

            var salesManagement = new SalesAppService(productRepository.Object, orderRepository.Object, customerRepository.Object, _mockLogger.Object);

            var order = new OrderDTO() // order is not valid when customer id is empty
            {
                CustomerId = Guid.Empty
            };
            
            Exception ex = Assert.Throws<ArgumentException>(() =>
            {
                //act
                var result = salesManagement.AddNewOrder(order);

                //assert
                Assert.Null(result);
            }
            );

            Assert.IsType(typeof(ArgumentException), ex);
        }
        [Fact]
        public void AddNewValidOrderReturnAddedOrder()
        {
            //Arrange 
            
            var productRepository = new Mock<IProductRepository>();
            var orderRepository = new Mock<IOrderRepository>();
            var customerRepository = new Mock<ICustomerRepository>();
            var country = new Country("Spain", "es-ES");
            country.GenerateNewIdentity();

            customerRepository
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .Returns((Guid guid) => {
                    //default credit limit is 1000
                    var customer = CustomerFactory.CreateCustomer("Jhon", "El rojo", "+34343", "company", country, new Address("city", "zipCode", "addressline1", "addressline2"));

                    return customer;
                });

            Mock<MainBCUnitOfWork> _mockContext = new Mock<MainBCUnitOfWork>();
            _mockContext.Setup(c => c.Commit());

            orderRepository.Setup(x => x.Add(It.IsAny<Order>()));

            orderRepository
                .Setup(x => x.UnitOfWork).Returns(_mockContext.Object);

            Mock<ILogger<SalesAppService>> _mockLogger = new Mock<ILogger<SalesAppService>>();
            
            var salesManagement = new SalesAppService(productRepository.Object, orderRepository.Object, customerRepository.Object, _mockLogger.Object);

            var dto = new OrderDTO()
            {
                CustomerId = Guid.NewGuid(),
                ShippingAddress = "Address",
                ShippingCity = "city",
                ShippingName = "name",
                ShippingZipCode = "zipcode",
                OrderLines = new List<OrderLineDTO>()
                {
                    new OrderLineDTO(){ProductId = Guid.NewGuid(),Amount = 1,Discount = 0,UnitPrice = 20}
                }
            };

            //act
            var result = salesManagement.AddNewOrder(dto);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Id != Guid.Empty);
            Assert.Equal(result.ShippingAddress, dto.ShippingAddress);
            Assert.Equal(result.ShippingCity, dto.ShippingCity);
            Assert.Equal(result.ShippingName, dto.ShippingName);
            Assert.Equal(result.ShippingZipCode, dto.ShippingZipCode);
            Assert.True(result.OrderLines.Count == 1);
            Assert.True(result.OrderLines.All(ol => ol.Id != Guid.Empty));
        }
        [Fact]
        public void AddNewOrderWithTotalGreaterCustomerCreditReturnNull()
        {
            //Arrange 
            var productRepository = new Mock<IProductRepository>();
            var orderRepository = new Mock<IOrderRepository>();
            var customerRepository = new Mock<ICustomerRepository>();
            var country = new Country("spain", "es-ES");
            country.GenerateNewIdentity();

            customerRepository
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .Returns((Guid guid) => {
                    //default credit limit is 1000
                    var customer = CustomerFactory.CreateCustomer("Jhon", "El rojo", "+34343", "company", country, new Address("city", "zipCode", "addressline1", "addressline2"));

                    return customer;
                });

            Mock<MainBCUnitOfWork> _mockContext = new Mock<MainBCUnitOfWork>();
            _mockContext.Setup(c => c.Commit());

            orderRepository.Setup(x => x.Add(It.IsAny<Order>()));

            orderRepository
                .Setup(x => x.UnitOfWork).Returns(_mockContext.Object);

            Mock<ILogger<SalesAppService>> _mockLogger = new Mock<ILogger<SalesAppService>>();
            
            var salesManagement = new SalesAppService(productRepository.Object, orderRepository.Object, customerRepository.Object, _mockLogger.Object);

            var dto = new OrderDTO()
            {
                CustomerId = Guid.NewGuid(),
                ShippingAddress = "Address",
                ShippingCity = "city",
                ShippingName = "name",
                ShippingZipCode = "zipcode",
                OrderLines = new List<OrderLineDTO>()
                {
                    new OrderLineDTO(){ProductId = Guid.NewGuid(),Amount = 1,Discount = 0,UnitPrice = 2000}
                }
            };

            //act
            var result = salesManagement.AddNewOrder(dto);

            //assert
            Assert.Null(result);
        }

        [Fact]
        public void AddNewSoftwareWithNullDataThrowArgumentException()
        {
            //Arrange 
            var customerRepository = new Mock<ICustomerRepository>();
            var productRepository = new Mock<IProductRepository>();
            var orderRepository = new Mock<IOrderRepository>();
            Mock<ILogger<SalesAppService>> _mockLogger = new Mock<ILogger<SalesAppService>>();
            var salesManagement = new SalesAppService(productRepository.Object, orderRepository.Object, customerRepository.Object, _mockLogger.Object);
            
            Exception ex = Assert.Throws<ArgumentException>(() =>
            {
                //Act
                var result = salesManagement.AddNewSoftware(null);
            }
            );

            Assert.IsType(typeof(ArgumentException), ex);
        }
        [Fact]
        public void AddNewSoftwareThrowExceptionWhenDataIsInvalid()
        {
            //Arrange 
            var customerRepository = new Mock<ICustomerRepository>();
            var productRepository = new Mock<IProductRepository>();
            var orderRepository = new Mock<IOrderRepository>();
            Mock<ILogger<SalesAppService>> _mockLogger = new Mock<ILogger<SalesAppService>>();

            var salesManagement = new SalesAppService(productRepository.Object, orderRepository.Object, customerRepository.Object, _mockLogger.Object);

            var dto = new SoftwareDTO()
            {
                Title = "The title",
                Description = "the description",
                LicenseCode = "license",
                AmountInStock = 10,
                UnitPrice = -1//this is a not valid value
            };
            
            Exception ex = Assert.Throws<ApplicationValidationErrorsException>(() =>
            {
                //Act
                var result = salesManagement.AddNewSoftware(dto);
            }
            );

            Assert.IsType(typeof(ApplicationValidationErrorsException), ex);
        }
        [Fact]
        public void AddNewBookWithNullDataThrowArgumentException()
        {
            //Arrange 
            var customerRepository = new Mock<ICustomerRepository>();
            var productRepository = new Mock<IProductRepository>();
            var orderRepository = new Mock<IOrderRepository>();
            Mock<ILogger<SalesAppService>> _mockLogger = new Mock<ILogger<SalesAppService>>();

            var salesManagement = new SalesAppService(productRepository.Object, orderRepository.Object, customerRepository.Object, _mockLogger.Object);

            Exception ex = Assert.Throws<ArgumentNullException>(() =>
            {
                //Act

                var result = salesManagement.AddNewBook(null);

                //Assert
                Assert.Null(result);
            }
            );

            Assert.IsType(typeof(ArgumentNullException), ex);
        }
        [Fact]
        public void AddNewBookThrowExceptionWhenDataIsInvalid()
        {
            //Arrange 
            
            var customerRepository = new Mock<ICustomerRepository>();
            var productRepository = new Mock<IProductRepository>();
            var orderRepository = new Mock<IOrderRepository>();
            Mock<ILogger<SalesAppService>> _mockLogger = new Mock<ILogger<SalesAppService>>();

            var salesManagement = new SalesAppService(productRepository.Object, orderRepository.Object, customerRepository.Object, _mockLogger.Object);

            var dto = new BookDTO()
            {
                Title = "The title",
                Description = "description",
                Publisher = "license",
                ISBN = "isbn",
                AmountInStock = 10,
                UnitPrice = -1//this is a not valid value
            };

            Exception ex = Assert.Throws<ApplicationValidationErrorsException>(() =>
            {
                //Act
                var result = salesManagement.AddNewBook(dto);
            }
            );

            Assert.IsType(typeof(ApplicationValidationErrorsException), ex);

        }
        [Fact]
        public void AddNewBookReturnAddedBook()
        {
            //Arrange 
            var customerRepository = new Mock<ICustomerRepository>();
            var orderRepository = new Mock<IOrderRepository>();
            var productRepository = new Mock<IProductRepository>();

            Mock<MainBCUnitOfWork> _mockContext = new Mock<MainBCUnitOfWork>();
            _mockContext.Setup(c => c.Commit());

            productRepository.Setup(x => x.Add(It.IsAny<Product>()));

            productRepository
                .Setup(x => x.UnitOfWork).Returns(_mockContext.Object);

            Mock<ILogger<SalesAppService>> _mockLogger = new Mock<ILogger<SalesAppService>>();

            var salesManagement = new SalesAppService(productRepository.Object, orderRepository.Object, customerRepository.Object, _mockLogger.Object);

            var dto = new BookDTO()
            {
                Title = "The title",
                Description = "description",
                Publisher = "license",
                ISBN = "isbn",
                AmountInStock = 10,
                UnitPrice = 10
            };

            //Act
            var result = salesManagement.AddNewBook(dto);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Id != Guid.Empty);
            Assert.Equal(result.Title, dto.Title);
            Assert.Equal(result.Description, dto.Description);
            Assert.Equal(result.Publisher, dto.Publisher);
            Assert.Equal(result.ISBN, dto.ISBN);
            Assert.Equal(result.AmountInStock, dto.AmountInStock);
            Assert.Equal(result.UnitPrice, dto.UnitPrice);
        }
        [Fact]
        public void AddNewSoftwareReturnAddedSoftware()
        {
            //Arrange 
            var customerRepository = new Mock<ICustomerRepository>();
            var orderRepository = new Mock<IOrderRepository>();
            var productRepository = new Mock<IProductRepository>();

            Mock<MainBCUnitOfWork> _mockContext = new Mock<MainBCUnitOfWork>();
            _mockContext.Setup(c => c.Commit());

            productRepository.Setup(x => x.Add(It.IsAny<Product>()));

            productRepository
                .Setup(x => x.UnitOfWork).Returns(_mockContext.Object);

            Mock<ILogger<SalesAppService>> _mockLogger = new Mock<ILogger<SalesAppService>>();

            var salesManagement = new SalesAppService(productRepository.Object, orderRepository.Object, customerRepository.Object, _mockLogger.Object);

            var dto = new SoftwareDTO()
            {
                Title = "The title",
                Description = "description",
                LicenseCode = "license code",
                AmountInStock = 10,
                UnitPrice = 10
            };

            //Act
            var result = salesManagement.AddNewSoftware(dto);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Id != Guid.Empty);
            Assert.Equal(result.Title, dto.Title);
            Assert.Equal(result.Description, dto.Description);
            Assert.Equal(result.LicenseCode, dto.LicenseCode);
            Assert.Equal(result.AmountInStock, dto.AmountInStock);
            Assert.Equal(result.UnitPrice, dto.UnitPrice);
        }

        [Fact]
        public void ConstructorThrowExceptionIfOrderRepositoryDependencyIsNull()
        {
            //Arrange
            var customerRepository = new Mock<ICustomerRepository>();
            var productRepository = new Mock<IProductRepository>();
            Mock<ILogger<SalesAppService>> _mockLogger = new Mock<ILogger<SalesAppService>>();
            
            Exception ex = Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                var salesManagement = new SalesAppService(productRepository.Object, null, customerRepository.Object, _mockLogger.Object);
            }
            );

            Assert.IsType(typeof(ArgumentNullException), ex);
        }
        [Fact]
        public void ConstructorThrowExceptionIfProductRepositoryDependencyIsNull()
        {
            //Arrange
            var customerRepository = new Mock<ICustomerRepository>();
            var orderRepository = new Mock<IOrderRepository>();
            Mock<ILogger<SalesAppService>> _mockLogger = new Mock<ILogger<SalesAppService>>();
            
            Exception ex = Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                var salesManagement = new SalesAppService(null, orderRepository.Object, customerRepository.Object, _mockLogger.Object);
            }
            );

            Assert.IsType(typeof(ArgumentNullException), ex);
        }
        [Fact]
        public void ConstructorThrowExceptionIfCustomerRepositoryDependencyIsNull()
        {
            //Arrange
            var orderRepository = new Mock<IOrderRepository>();
            var productRepository = new Mock<IProductRepository>();
            Mock<ILogger<SalesAppService>> _mockLogger = new Mock<ILogger<SalesAppService>>();
            
            Exception ex = Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                var salesManagement = new SalesAppService(productRepository.Object, orderRepository.Object, null, _mockLogger.Object);
            }
            );

            Assert.IsType(typeof(ArgumentNullException), ex);
        }
    }
}

