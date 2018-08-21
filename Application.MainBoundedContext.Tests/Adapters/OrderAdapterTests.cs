namespace NLayerApp.Application.MainBoundedContext.Tests
{
    using NLayerApp.Application.MainBoundedContext.DTO;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
    using NLayerApp.Infrastructure.Crosscutting.Adapter;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    [Collection("Our Test Collection #2")]

    public class OrderAdapterTests 
    {
        protected TestsInitialize fixture;

        public OrderAdapterTests(TestsInitialize fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void OrderToOrderDTOAdapter()
        {
            //Arrange

            Customer customer = new Customer();
            customer.GenerateNewIdentity();
            customer.FirstName = "Unai";
            customer.LastName = "Zorrilla";

            Product product = new Software("the product title", "the product description","license code");
            product.GenerateNewIdentity();
            

            Order order = new Order();
            order.GenerateNewIdentity();
            order.OrderDate = DateTime.Now;
            order.ShippingInformation = new ShippingInfo("shippingName", "shippingAddress", "shippingCity", "shippingZipCode");
            order.SetTheCustomerForThisOrder(customer);

            var orderLine = order.AddNewOrderLine(product.Id, 10, 10, 0.5M);
            orderLine.SetProduct(product);

            //Act
            ITypeAdapter adapter = TypeAdapterFactory.CreateAdapter();
            var orderDTO = adapter.Adapt<Order, OrderDTO>(order);

            //Assert
            Assert.Equal(orderDTO.Id, order.Id);
            Assert.Equal(orderDTO.OrderDate, order.OrderDate);
            Assert.Equal(orderDTO.DeliveryDate, order.DeliveryDate);

            Assert.Equal(orderDTO.ShippingAddress, order.ShippingInformation.ShippingAddress);
            Assert.Equal(orderDTO.ShippingCity, order.ShippingInformation.ShippingCity);
            Assert.Equal(orderDTO.ShippingName, order.ShippingInformation.ShippingName);
            Assert.Equal(orderDTO.ShippingZipCode, order.ShippingInformation.ShippingZipCode);

            Assert.Equal(orderDTO.CustomerFullName, order.Customer.FullName);
            Assert.Equal(orderDTO.CustomerId, order.Customer.Id);

            Assert.Equal(orderDTO.OrderNumber, string.Format("{0}/{1}-{2}",order.OrderDate.Year,order.OrderDate.Month,order.SequenceNumberOrder));


            Assert.NotNull(orderDTO.OrderLines);
            Assert.True(orderDTO.OrderLines.Any());

            Assert.Equal(orderDTO.OrderLines[0].Id, orderLine.Id);
            Assert.Equal(orderDTO.OrderLines[0].Amount, orderLine.Amount);
            Assert.Equal(orderDTO.OrderLines[0].Discount, orderLine.Discount * 100);
            Assert.Equal(orderDTO.OrderLines[0].UnitPrice, orderLine.UnitPrice);
            Assert.Equal(orderDTO.OrderLines[0].TotalLine, orderLine.TotalLine);
            Assert.Equal(orderDTO.OrderLines[0].ProductId, product.Id);
            Assert.Equal(orderDTO.OrderLines[0].ProductTitle, product.Title);

        }

        [Fact]
        public void EnumerableOrderToOrderListDTOAdapter()
        {
            //Arrange

            Customer customer = new Customer();
            customer.GenerateNewIdentity();
            customer.FirstName = "Unai";
            customer.LastName = "Zorrilla";

            Product product = new Software("the product title", "the product description","license code");
            product.GenerateNewIdentity();


            Order order = new Order();
            order.GenerateNewIdentity();
            order.OrderDate = DateTime.Now;
            order.ShippingInformation = new ShippingInfo("shippingName", "shippingAddress", "shippingCity", "shippingZipCode");
            order.SetTheCustomerForThisOrder(customer);

            var line = order.AddNewOrderLine(product.Id, 1, 200, 0);
            

            var orders = new List<Order>() { order };

            //Act
            ITypeAdapter adapter = TypeAdapterFactory.CreateAdapter();
            var orderListDTO = adapter.Adapt<IEnumerable<Order>, List<OrderListDTO>>(orders);

            //Assert
            Assert.Equal(orderListDTO[0].Id, order.Id);
            Assert.Equal(orderListDTO[0].OrderDate, order.OrderDate);
            Assert.Equal(orderListDTO[0].DeliveryDate, order.DeliveryDate);
            Assert.Equal(orderListDTO[0].TotalOrder, order.GetOrderTotal());

            Assert.Equal(orderListDTO[0].ShippingAddress, order.ShippingInformation.ShippingAddress);
            Assert.Equal(orderListDTO[0].ShippingCity, order.ShippingInformation.ShippingCity);
            Assert.Equal(orderListDTO[0].ShippingName, order.ShippingInformation.ShippingName);
            Assert.Equal(orderListDTO[0].ShippingZipCode, order.ShippingInformation.ShippingZipCode);

            Assert.Equal(orderListDTO[0].CustomerFullName, order.Customer.FullName);
            Assert.Equal(orderListDTO[0].CustomerId, order.Customer.Id);
        }
    }
}
