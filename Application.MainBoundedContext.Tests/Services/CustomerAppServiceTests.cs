namespace NLayerApp.Application.MainBoundedContext.Tests
{
    using Microsoft.Extensions.Logging;
    using Moq;
    using NLayerApp.Application.MainBoundedContext.DTO;
    using NLayerApp.Application.MainBoundedContext.ERPModule.Services;
    using NLayerApp.Application.Seedwork;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using NLayerApp.Domain.Seedwork.Specification;
    using NLayerApp.Infrastructure.Data.MainBoundedContext.UnitOfWork;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Xunit;

    [Collection("Our Test Collection #2")]

    public class CustomerAppServiceTests 
    {
        protected TestsInitialize fixture;

        public CustomerAppServiceTests(TestsInitialize fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void AddNewCustomerThrowExceptionIfCustomerDtoIsNull()
        {
            //Arrange
            var countryRepository = new Mock<ICountryRepository>();
            var customerRepository = new Mock<ICustomerRepository>();
            Mock<ILogger<CustomerAppService>> _mockLogger = new Mock<ILogger<CustomerAppService>>();
            var customerManagementService = new CustomerAppService(countryRepository.Object, customerRepository.Object, _mockLogger.Object);

            Exception ex = Assert.Throws<ArgumentException>(() =>
            {
                //act
                var result = customerManagementService.AddNewCustomer(null);

                //Assert
                Assert.Null(result);
            }
            );

            Assert.IsType(typeof(ArgumentException), ex);
            
        }

        [Fact]
        public void AddNewCustomerThrowArgumentExceptionIfCustomerCountryInformationIsEmpty()
        {
            //Arrange
            var countryRepository = new Mock<ICountryRepository>();
            var customerRepository = new Mock<ICustomerRepository>();
            Mock<ILogger<CustomerAppService>> _mockLogger = new Mock<ILogger<CustomerAppService>>();
            var customerManagementService = new CustomerAppService(countryRepository.Object, customerRepository.Object, _mockLogger.Object);

            var customerDTO = new CustomerDTO()
            {
                CountryId = Guid.Empty
            };

            Exception ex = Assert.Throws<ArgumentException>(() =>
            {
                //act
                var result = customerManagementService.AddNewCustomer(customerDTO);
            }
            );

            Assert.IsType(typeof(ArgumentException), ex);
        }

        [Fact]
        public void AddNewCustomerReturnAdaptedDTO()
        {
            //Arrange
            var countryRepository = new Mock<ICountryRepository>();

            countryRepository
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .Returns((Guid guid) => {
                    var country = new Country("Spain", "es-ES"); ;
                    country.ChangeCurrentIdentity(guid);

                    return country;
                });

            Mock<MainBCUnitOfWork> _mockContext = new Mock<MainBCUnitOfWork>();
            _mockContext.Setup(c => c.Commit());

            var customerRepository = new Mock<ICustomerRepository>();
            customerRepository.Setup(x => x.Add(It.IsAny<Customer>()));

            customerRepository
                .Setup(x => x.UnitOfWork).Returns(_mockContext.Object);

            Mock<ILogger<CustomerAppService>> _mockLogger = new Mock<ILogger<CustomerAppService>>();

            var customerManagementService = new CustomerAppService(countryRepository.Object, customerRepository.Object, _mockLogger.Object);

            var customerDTO = new CustomerDTO()
            {
                CountryId = Guid.NewGuid(),
                FirstName = "Jhon",
                LastName = "El rojo"
            };

            //act
            var result = customerManagementService.AddNewCustomer(customerDTO);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Id != Guid.Empty);
            Assert.Equal(result.FirstName, customerDTO.FirstName);
            Assert.Equal(result.LastName, customerDTO.LastName);
        }


        [Fact]
        public void AddNewCustomerThrowApplicationErrorsWhenEntityIsNotValid()
        {
            //Arrange
            var countryId = Guid.NewGuid();

            var countryRepository = new Mock<ICountryRepository>();

            countryRepository
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .Returns((Guid guid) => {
                    var country = new Country("spain", "es-ES");
                    country.GenerateNewIdentity();

                    return country;
                });


            var customerRepository = new Mock<ICustomerRepository>();
            Mock<MainBCUnitOfWork> _mockContext = new Mock<MainBCUnitOfWork>();
            _mockContext.Setup(c => c.Commit());

            customerRepository.Setup(x => x.Add(It.IsAny<Customer>()));

            customerRepository
                .Setup(x => x.UnitOfWork).Returns(_mockContext.Object);

            Mock<ILogger<CustomerAppService>> _mockLogger = new Mock<ILogger<CustomerAppService>>();

            var customerManagementService = new CustomerAppService(countryRepository.Object, customerRepository.Object, _mockLogger.Object);

            var customerDTO = new CustomerDTO() //missing lastname
            {
                CountryId = Guid.NewGuid(),
                FirstName = "Jhon"
            };
            
            Exception ex = Assert.Throws<ApplicationValidationErrorsException>(() =>
            {
                //act
                var result = customerManagementService.AddNewCustomer(customerDTO);
            }
            );

            Assert.IsType(typeof(ApplicationValidationErrorsException), ex);
        }
        [Fact]
        public void RemoveCustomerSetCustomerAsDisabled()
        {
            //Arrange
            var country = new Country("spain", "es-ES");
            country.GenerateNewIdentity();

            Guid customerId = Guid.NewGuid();
            
            var countryRepository = new Mock<ICountryRepository>();
            var customerRepository = new Mock<ICustomerRepository>();
            Mock<MainBCUnitOfWork> _mockContext = new Mock<MainBCUnitOfWork>();
            _mockContext.Setup(c => c.Commit());
            customerRepository
                .Setup(x => x.UnitOfWork).Returns(_mockContext.Object);

            var customer = CustomerFactory.CreateCustomer("Jhon", "El rojo","+3434","company",country, new Address("city", "zipCode", "address line", "address line"));
            customer.ChangeCurrentIdentity(customerId);

            customerRepository
            .Setup(x => x.Get(It.IsAny<Guid>()))
            .Returns((Guid guid) => {
                return customer;
            });

            Mock<ILogger<CustomerAppService>> _mockLogger = new Mock<ILogger<CustomerAppService>>();

            //Act
            var customerManagementService = new CustomerAppService(countryRepository.Object, customerRepository.Object, _mockLogger.Object);
            customerManagementService.RemoveCustomer(customerId);

            //Assert
            Assert.False(customer.IsEnabled);
        }

        [Fact]
        public void UpdateCustomerMergePersistentAndCurrent()
        {
            //Arrange
            var country = new Country("spain", "es-ES");
            country.GenerateNewIdentity();

            Guid customerId = Guid.NewGuid();
            
            var countryRepository = new Mock<ICountryRepository>();
            var customerRepository = new Mock<ICustomerRepository>();
            Mock<MainBCUnitOfWork> _mockContext = new Mock<MainBCUnitOfWork>();
            _mockContext.Setup(c => c.Commit());
            customerRepository
                .Setup(x => x.UnitOfWork).Returns(_mockContext.Object);

            customerRepository
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .Returns((Guid guid) => {
                    var customer = CustomerFactory.CreateCustomer("Jhon",
                                                               "El rojo",
                                                               "+3434",
                                                               "company",
                                                               country,
                                                               new Address("city", "zipCode", "address line", "address line"));
                    customer.ChangeCurrentIdentity(customerId);

                    return customer;
                });

            customerRepository
                .Setup(x => x.Merge(It.IsAny<Customer>(), It.IsAny<Customer>()))
                .Callback<Customer, Customer>((persistent, current) =>
                {
                    Assert.Equal(persistent, current);
                    Assert.True(persistent != null);
                    Assert.True(current != null);
                }
                );
            
            Mock<ILogger<CustomerAppService>> _mockLogger = new Mock<ILogger<CustomerAppService>>();

            var customerManagementService = new CustomerAppService(countryRepository.Object, customerRepository.Object, _mockLogger.Object);

            var customerDTO = new CustomerDTO() //missing lastname
            {
                Id = customerId,
                CountryId = country.Id,
                FirstName = "Jhon",
                LastName = "El rojo",
            };

            //act
            customerManagementService.UpdateCustomer(customerDTO);
        }
        [Fact]
        public void FindCustomersWithInvalidPageArgumentsThrowArgumentException()
        {
            //Arrange
            var countryRepository = new Mock<ICountryRepository>();
            var customerRepository = new Mock<ICustomerRepository>();

            Mock<ILogger<CustomerAppService>> _mockLogger = new Mock<ILogger<CustomerAppService>>();

            var customerManagementService = new CustomerAppService(countryRepository.Object, customerRepository.Object, _mockLogger.Object);
            
            Exception ex = Assert.Throws<ArgumentException>(() =>
            {
                //Act
                customerManagementService.FindCustomers(-1, 0);
            }
            );

            Assert.IsType(typeof(ArgumentException), ex);
        }
        [Fact]
        public void FindCustomersInPageReturnNullIfNotData()
        {
            //Arrange
            var countryRepository = new Mock<ICountryRepository>();
            var customerRepository = new Mock<ICustomerRepository>();

            customerRepository
                .Setup(x => x.GetEnabled(It.IsAny<Int32>(), It.IsAny<Int32>()))
                .Returns((Int32 index, Int32 count) => {
                    return new List<Customer>();
                });

            Mock<ILogger<CustomerAppService>> _mockLogger = new Mock<ILogger<CustomerAppService>>();

            var customerManagementService = new CustomerAppService(countryRepository.Object, customerRepository.Object, _mockLogger.Object);

            //Act
            var result = customerManagementService.FindCustomers(0, 1);

            //Assert
            Assert.Null(result);

        }
        [Fact]
        public void FindCustomersByFilterReturnNullIfNotData()
        {
            //Arrange
            var countryRepository = new Mock<ICountryRepository>();
            var customerRepository = new Mock<ICustomerRepository>();
            
            customerRepository
                .Setup(x => x.AllMatching(It.IsAny<ISpecification<Customer>>()))
                .Returns((ISpecification<Customer> spec) => {
                    return new List<Customer>();
                });

            Mock<ILogger<CustomerAppService>> _mockLogger = new Mock<ILogger<CustomerAppService>>();

            var customerManagementService = new CustomerAppService(countryRepository.Object, customerRepository.Object, _mockLogger.Object);

            //Act
            var result = customerManagementService.FindCustomers("text");

            //Assert
            Assert.Null(result);

        }
        [Fact]
        public void FindCustomersInPageMaterializeResults()
        {
            //Arrange
            var countryRepository = new Mock<ICountryRepository>();
            var customerRepository = new Mock<ICustomerRepository>();
            var country = new Country("spain", "es-ES");
            country.GenerateNewIdentity();

            customerRepository
                .Setup(x => x.GetEnabled(It.IsAny<Int32>(), It.IsAny<Int32>()))
                .Returns((Int32 index, Int32 count) => {
                    var customers = new List<Customer>();
                    customers.Add(CustomerFactory.CreateCustomer("Jhon",
                                                                "El rojo",
                                                                "+343",
                                                                "company",
                                                                 country,
                                                                 new Address("city", "zipCode", "address line", "address line2")));
                    return customers;
                });

            Mock<ILogger<CustomerAppService>> _mockLogger = new Mock<ILogger<CustomerAppService>>();

            var customerManagementService = new CustomerAppService(countryRepository.Object, customerRepository.Object, _mockLogger.Object);

            //Act
            var result = customerManagementService.FindCustomers(0, 1);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 1);
        }

        [Fact]
        public void FindCustomersByFilterMaterializeResults()
        {
            //Arrange
            var countryRepository = new Mock<ICountryRepository>();
            var customerRepository = new Mock<ICustomerRepository>();
            var country = new Country("Spain", "es-ES");
            country.GenerateNewIdentity();

            customerRepository
                .Setup(x => x.AllMatching(It.IsAny<ISpecification<Customer>>()))
                .Returns((ISpecification<Customer> spec) => {
                    var customers = new List<Customer>();
                    customers.Add(CustomerFactory.CreateCustomer("Jhon",
                                                                "El rojo",
                                                                "+34343",
                                                                "company",
                                                                 country,
                                                                 new Address("city", "zipCode", "address line", "address line2")));
                    return customers;
                });

            Mock<ILogger<CustomerAppService>> _mockLogger = new Mock<ILogger<CustomerAppService>>();

            var customerManagementService = new CustomerAppService(countryRepository.Object, customerRepository.Object, _mockLogger.Object);

            //Act
            var result = customerManagementService.FindCustomers("Jhon");

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 1);
        }
        [Fact]
        public void FindCustomerReturnNullIfCustomerIdIsEmpty()
        {
            //Arrange
            var countryRepository = new Mock<ICountryRepository>();
            var customerRepository = new Mock<ICustomerRepository>();

            customerRepository
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .Returns((Guid guid) => {
                    if (guid == Guid.Empty)
                        return null;
                    else
                        return new Customer();
                });

            Mock<ILogger<CustomerAppService>> _mockLogger = new Mock<ILogger<CustomerAppService>>();

            var customerManagementService = new CustomerAppService(countryRepository.Object, customerRepository.Object, _mockLogger.Object);

            //Act
            var result = customerManagementService.FindCustomer(Guid.Empty);

            //Assert
            Assert.Null(result);

        }
        [Fact]
        public void FindCustomerMaterializaResultIfExist()
        {
            //Arrange
            var countryRepository = new Mock<ICountryRepository>();
            var customerRepository = new Mock<ICustomerRepository>();
            var country = new Country("spain", "es-ES");
            country.GenerateNewIdentity();

            customerRepository
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .Returns((Guid guid) => {
                    return CustomerFactory.CreateCustomer("Jhon",
                                                      "El rojo",
                                                      "+3434344",
                                                      "company",
                                                      country,
                                                      new Address("city", "zipCode", "address line1", "address line2"));
                });

            Mock<ILogger<CustomerAppService>> _mockLogger = new Mock<ILogger<CustomerAppService>>();

            var customerManagementService = new CustomerAppService(countryRepository.Object, customerRepository.Object, _mockLogger.Object);

            //Act
            var result = customerManagementService.FindCustomer(Guid.NewGuid());

            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public void FindCountriesWithInvalidPageArgumentsReturnNull()
        {
            //Arrange
            var countryRepository = new Mock<ICountryRepository>();
            var customerRepository = new Mock<ICustomerRepository>();
            Mock<ILogger<CustomerAppService>> _mockLogger = new Mock<ILogger<CustomerAppService>>();

            var customerManagementService = new CustomerAppService(countryRepository.Object, customerRepository.Object, _mockLogger.Object);
            
            Exception ex = Assert.Throws<ArgumentException>(() =>
            {
                //Act
                customerManagementService.FindCountries(-1, 0);
            }
            );

            Assert.IsType(typeof(ArgumentException), ex);
        }
        [Fact]
        public void FindCountriesInPageReturnNullIfNotData()
        {
            //Arrange
            var customerRepository = new Mock<ICustomerRepository>();
            var countryRepository = new Mock<ICountryRepository>();
            Mock<ILogger<CustomerAppService>> _mockLogger = new Mock<ILogger<CustomerAppService>>();

            countryRepository
                .Setup(x => x.GetPaged<string>(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Expression<Func<Country,string>>>(), It.IsAny<bool>()))
                .Returns((Int32 index, Int32 count, Expression<Func<Country,string>> order, bool ascending) => {
                    return new List<Country>();
                });

            var customerManagementService = new CustomerAppService(countryRepository.Object, customerRepository.Object, _mockLogger.Object);

            //Act
            var result = customerManagementService.FindCountries(0, 1);

            //Assert
            Assert.Null(result);
        }
        [Fact]
        public void FindCountriesInPageMaterializeResults()
        {
            //Arrange
            var customerRepository = new Mock<ICustomerRepository>();
            var countryRepository = new Mock<ICountryRepository>();
            Mock<ILogger<CustomerAppService>> _mockLogger = new Mock<ILogger<CustomerAppService>>();

            countryRepository
                .Setup(x => x.GetPaged<string>(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Expression<Func<Country, string>>>(), It.IsAny<bool>()))
                .Returns((Int32 index, Int32 count, Expression<Func<Country, string>> order, bool ascending) => {
                    var country = new Country("country name", "country iso");
                    country.GenerateNewIdentity();

                    return new List<Country>()
                    {
                        country
                    };
                });

            var customerManagementService = new CustomerAppService(countryRepository.Object, customerRepository.Object, _mockLogger.Object);

            //Act
            var result = customerManagementService.FindCountries(0, 1);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 1);
        }
        [Fact]
        public void FindCountriesByFilterMaterializeResults()
        {
            //Arrange
            var customerRepository = new Mock<ICustomerRepository>();
            var countryRepository = new Mock<ICountryRepository>();
            Mock<ILogger<CustomerAppService>> _mockLogger = new Mock<ILogger<CustomerAppService>>();

            countryRepository
             .Setup(x => x.AllMatching(It.IsAny<ISpecification<Country>>()))
             .Returns((ISpecification<Country> spec) => {
                 var country = new Country("country name", "country iso");
                 country.GenerateNewIdentity();

                 return new List<Country>()
                    {
                       country
                    };
             });

            var customerManagementService = new CustomerAppService(countryRepository.Object, customerRepository.Object, _mockLogger.Object);

            //Act
            var result = customerManagementService.FindCountries("filter");

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 1);
        }
        [Fact]
        public void FindCountriesByFilterReturnNullIfNotData()
        {
            //Arrange
            var customerRepository = new Mock<ICustomerRepository>();
            var countryRepository = new Mock<ICountryRepository>();
            Mock<ILogger<CustomerAppService>> _mockLogger = new Mock<ILogger<CustomerAppService>>();

            countryRepository
             .Setup(x => x.AllMatching(It.IsAny<ISpecification<Country>>()))
             .Returns((ISpecification<Country> spec) => {
                 return new List<Country>();
             });
            var customerManagementService = new CustomerAppService(countryRepository.Object, customerRepository.Object, _mockLogger.Object);

            //Act
            var result = customerManagementService.FindCountries("filter");

            //Assert
            Assert.Null(result);
        }
        
        [Fact]
        public void ConstructorThrowExceptionWhenCustomerRepositoryDependencyIsNull()
        {
            //Arrange
            var countryRepository = new Mock<ICountryRepository>();
            Mock<ILogger<CustomerAppService>> _mockLogger = new Mock<ILogger<CustomerAppService>>();

            Exception ex = Assert.Throws<ArgumentNullException>(() =>
            {
                //act
                var customerManagementService = new CustomerAppService(countryRepository.Object, null, _mockLogger.Object);
            }
            );

            Assert.IsType(typeof(ArgumentNullException), ex);
        }
        [Fact]
        public void ConstructorThrowExceptionWhenCountryRepositoryDependencyIsNull()
        {
            //Arrange
            var customerRepository = new Mock<ICustomerRepository>();
            Mock<ILogger<CustomerAppService>> _mockLogger = new Mock<ILogger<CustomerAppService>>();
            
            Exception ex = Assert.Throws<ArgumentNullException>(() =>
            {
                //act
                var customerManagementService = new CustomerAppService(null, customerRepository.Object, _mockLogger.Object);
            }
            );

            Assert.IsType(typeof(ArgumentNullException), ex);

        }
    }
}
