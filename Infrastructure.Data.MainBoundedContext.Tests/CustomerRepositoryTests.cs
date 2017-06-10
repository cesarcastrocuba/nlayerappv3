namespace NLayerApp.Infrastructure.Data.MainBoundedContext.Tests
{
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using NLayerApp.Infrastructure.Data.MainBoundedContext.ERPModule.Repositories;
    using System;
    using System.Linq;
    using Xunit;

    [Collection("Our Test Collection #1")]
    public class CustomerRepositoryTests : IClassFixture<MainBCUnitOfWorkInitializer>
    {
        protected MainBCUnitOfWorkInitializer fixture;
        public CustomerRepositoryTests(MainBCUnitOfWorkInitializer fixture
            )
        {
            this.fixture = fixture;
        }

        [Fact]
        public void CustomerRepositoryGetMethodReturnCustomerWithPicture()
        {
            //Arrange
            var customerRepository = new CustomerRepository(fixture.unitOfWork, fixture.customerLogger);

            var customerId = new Guid("43A38AC8-EAA9-4DF0-981F-2685882C7C45");
            
            //Act
            var customer = customerRepository.Get(customerId);

            //Assert
            Assert.NotNull(customer);
            Assert.NotNull(customer.Picture);
            Assert.True(customer.Id == customerId);
        }

        [Fact]
        public void CustomerRepositoryGetMethodReturnNullWhenIdIsEmpty()
        {
            //Arrange
            var customerRepository = new CustomerRepository(fixture.unitOfWork, fixture.customerLogger);

            //Act
            var customer = customerRepository.Get(Guid.Empty);

            //Assert
            Assert.Null(customer);
        }

        [Fact]
        public void CustomerRepositoryGetEnalbedReturnOnlyEnabledCustomers()
        {
            //Arrange
            var customerRepository = new CustomerRepository(fixture.unitOfWork, fixture.customerLogger);
            
            //Act
            var result = customerRepository.GetEnabled(0, 10);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.True(result.All(c => c.IsEnabled));
        }

        [Fact]
        public void CustomerRepositoryAddNewItemSaveItem()
        {
            //Arrange
            var customerRepository = new CustomerRepository(fixture.unitOfWork, fixture.customerLogger);

            var country = new Country("spain", "es-ES");
            country.ChangeCurrentIdentity(new Guid("32BB805F-40A4-4C37-AA96-B7945C8C385C"));

            var customer = CustomerFactory.CreateCustomer("Felix", "Trend","+3434","company", country, new Address("city", "zipCode", "addressLine1", "addressLine2"));
            customer.SetTheCountryReference(country.Id);


            //Act
            customerRepository.Add(customer);
            fixture.unitOfWork.Commit();
        }

        [Fact]
        public void CustomerRepositoryGetAllReturnMaterializedAllItems()
        {
            //Arrange
            var customerRepository = new CustomerRepository(fixture.unitOfWork, fixture.customerLogger);

            //Act
            var allItems = customerRepository.GetAll();

            //Assert
            Assert.NotNull(allItems);
            Assert.True(allItems.Any());
        }

        [Fact]
        public void CustomerRepositoryAllMatchingMethodReturnEntitiesWithSatisfiedCriteria()
        {
            //Arrange
            var customerRepository = new CustomerRepository(fixture.unitOfWork, fixture.customerLogger);
            
            var spec = CustomerSpecifications.EnabledCustomers();

            //Act
            var result = customerRepository.AllMatching(spec);

            //Assert
            Assert.NotNull(result.All(c => c.IsEnabled));

        }

        [Fact]
        public void CustomerRepositoryFilterMethodReturnEntitisWithSatisfiedFilter()
        {
            //Arrange
            var customerRepository = new CustomerRepository(fixture.unitOfWork, fixture.customerLogger);

            //Act
            var result = customerRepository.GetFiltered(c => c.CreditLimit > 0);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.All(c => c.CreditLimit>0));
        }

        [Fact]
        public void CustomerRepositoryPagedMethodReturnEntitiesInPageFashion()
        {
            //Arrange
            var customerRepository = new CustomerRepository(fixture.unitOfWork, fixture.customerLogger);

            //Act
            var pageI = customerRepository.GetPaged(0, 1, b => b.Id, false);
            var pageII = customerRepository.GetPaged(1, 1, b => b.Id, false);

            //Assert
            Assert.NotNull(pageI);
            Assert.True(pageI.Count() == 1);

            Assert.NotNull(pageII);
            Assert.True(pageII.Count() == 1);

            Assert.False(pageI.Intersect(pageII).Any());
        }
        [Fact]
        public void CustomerRepositoryRemoveItemDeleteIt()
        {
            //Arrange
            var customerRepository = new CustomerRepository(fixture.unitOfWork, fixture.customerLogger);

            var country = new Country("Spain","es-ES");
            country.ChangeCurrentIdentity(new Guid("32BB805F-40A4-4C37-AA96-B7945C8C385C"));

            var address =  new Address("city", "zipCode", "addressline1", "addressline2");

            var customer = CustomerFactory.CreateCustomer("Frank", "Frank","+3444","company", country,address);
            customer.SetTheCountryReference(country.Id);
            
            customerRepository.Add(customer);
            fixture.unitOfWork.Commit();

            //Act
            customerRepository.Remove(customer);
            fixture.unitOfWork.Commit();

            var result = customerRepository.Get(customer.Id);

            //Assert
            Assert.Null(result);
        }
    }
}
