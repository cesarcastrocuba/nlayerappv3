namespace NLayerApp.Infrastructure.Data.MainBoundedContext.Tests
{
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg;
    using NLayerApp.Infrastructure.Data.MainBoundedContext.ERPModule.Repositories;
    using System;
    using System.Linq;
    using Xunit;

    [Collection("Our Test Collection #1")]
    public class OrderRepositoryTests : IClassFixture<MainBCUnitOfWorkInitializer>
    {
        protected MainBCUnitOfWorkInitializer fixture;
        public OrderRepositoryTests(MainBCUnitOfWorkInitializer fixture
            )
        {
            this.fixture = fixture;
        }

        [Fact]
        public void OrderRepositoryGetMethodReturnMaterializedEntityById()
        {
            //Arrange
            var orderRepository = new OrderRepository(fixture.unitOfWork, fixture.orderLogger);

            var orderId = new Guid("3135513C-63FD-43E6-9697-6C6E5D8CE55B");

            //Act
            var order = orderRepository.Get(orderId);

            //Assert
            Assert.NotNull(order);
            Assert.True(order.Id == orderId);
        }

        [Fact]
        public void OrderRepositoryGetMethodReturnNullWhenIdIsEmpty()
        {
            //Arrange
            var orderRepository = new OrderRepository(fixture.unitOfWork, fixture.orderLogger);

            //Act
            var order = orderRepository.Get(Guid.Empty);

            //Assert
            Assert.Null(order);
        }

        [Fact]
        public void OrderRepositoryAddNewItemSaveItem()
        {
            //Arrange
            var customerRepository = new CustomerRepository(fixture.unitOfWork, fixture.customerLogger);
            var orderRepository = new OrderRepository(fixture.unitOfWork, fixture.orderLogger);

            var customer = customerRepository.Get(new Guid("0CD6618A-9C8E-4D79-9C6B-4AA69CF18AE6"));

            var order = OrderFactory.CreateOrder(customer, "shipping name", "shipping city", "shipping address", "shipping zip code");
            order.GenerateNewIdentity();

            //Act
            orderRepository.Add(order);
            fixture.unitOfWork.Commit();
        }

        [Fact]
        public void OrderRepositoryGetAllReturnMaterializedAllItems()
        {
            //Arrange
            var orderRepository = new OrderRepository(fixture.unitOfWork, fixture.orderLogger);

            //Act
            var allItems = orderRepository.GetAll();

            //Assert

            Assert.NotNull(allItems);
            Assert.True(allItems.Any());
        }

        [Fact]
        public void OrderRepositoryAllMatchingMethodReturnEntitiesWithSatisfiedCriteria()
        {
            //Arrange
            var orderRepository = new OrderRepository(fixture.unitOfWork, fixture.orderLogger);

            var spec = OrdersSpecifications.OrderFromDateRange(DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1));

            //Act
            var result = orderRepository.AllMatching(spec);

            //Assert
            Assert.NotNull(result.All(o => o.OrderDate > DateTime.Now.AddDays(-2) && o.OrderDate < DateTime.Now.AddDays(-1)));
        }

        [Fact]
        public void OrderRepositoryFilterMethodReturnEntitisWithSatisfiedFilter()
        {
            //Arrange
            var orderRepository = new OrderRepository(fixture.unitOfWork, fixture.orderLogger);

            //Act
            var result = orderRepository.GetFiltered(o => o.IsDelivered == false);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.All(o => o.IsDelivered));
        }

        [Fact]
        public void OrderRepositoryPagedMethodReturnEntitiesInPageFashion()
        {
            //Arrange
            var orderRepository = new OrderRepository(fixture.unitOfWork, fixture.orderLogger);

            //Act
            var pageI = orderRepository.GetPaged(0, 1, b => b.Id, false);
            var pageII = orderRepository.GetPaged(1, 1, b => b.Id, false);

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
            var customerRepository = new CustomerRepository(fixture.unitOfWork, fixture.customerLogger);
            var orderRepository = new OrderRepository(fixture.unitOfWork, fixture.orderLogger);

            var customer = customerRepository.Get(new Guid("0CD6618A-9C8E-4D79-9C6B-4AA69CF18AE6"));

            var order = OrderFactory.CreateOrder(customer, "shipping name", "shipping city", "shipping address", "shipping zip code");
            order.GenerateNewIdentity();

            orderRepository.Add(order);
            orderRepository.UnitOfWork.Commit();

            //Act
            orderRepository.Remove(order);
            fixture.unitOfWork.Commit();
        }
    }
}
