using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg;
using NLayerApp.Infrastructure.Crosscutting.Localization;
using NLayerApp.Infrastructure.Crosscutting.NetFramework.Localization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace Domain.MainBoundedContext.Tests
{
    public class OrderAggTests
    {
        public OrderAggTests()
        {
            //Localization
            LocalizationFactory.SetCurrent(new ResourcesManagerFactory());
        }

        [Fact]        
        public void OrderCannotSetTransientCustomer()
        {
            //Arrange 
            Customer customer = new Customer();

            Order order = new Order();

            //Act            
            Exception ex = Assert.Throws<ArgumentException>(() => order.SetTheCustomerForThisOrder(customer));
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]        
        public void OrderCannotSetNullCustomer()
        {
            //Arrange 
            Customer customer = new Customer();

            Order order = new Order();

            //Act            
            Exception ex = Assert.Throws<ArgumentException>(() => order.SetTheCustomerForThisOrder(customer));
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void OrderSetDeliveredSetDateAndState()
        {
            //Arrange 
            Order order = new Order();

            //Act
            order.SetOrderAsDelivered();

            //Assert
            Assert.True(order.IsDelivered);
            Assert.NotNull(order.DeliveryDate);
            Assert.True(order.DeliveryDate != default(DateTime));
        }
        [Fact]
        public void OrderAddNewOrderLineFixOrderId()
        {
            //Arrange
            string shippingName = "shippingName";
            string shippingCity = "shippingCity";
            string shippingZipCode = "shippingZipCode";
            string shippingAddress = "shippingAddress";

            Customer customer = new Customer();
            customer.GenerateNewIdentity();

            Order order = OrderFactory.CreateOrder(customer, shippingName, shippingCity, shippingAddress, shippingZipCode);
            order.GenerateNewIdentity();

            var line = order.AddNewOrderLine(Guid.NewGuid(), 1, 1, 0);

            //Assert
            Assert.Equal(order.Id, line.OrderId);
        }

        [Fact]
        public void OrderGetTotalOrderSumLines()
        {
            //Arrange
            string shippingName = "shippingName";
            string shippingCity = "shippingCity";
            string shippingZipCode = "shippingZipCode";
            string shippingAddress = "shippingAddress";

            Customer customer = new Customer();
            customer.GenerateNewIdentity();

            Order order = OrderFactory.CreateOrder(customer, shippingName, shippingCity, shippingAddress, shippingZipCode);

            order.AddNewOrderLine(Guid.NewGuid(), 1, 500, 10);
            order.AddNewOrderLine(Guid.NewGuid(), 2, 300, 10);

            decimal expected = ((1 * 500) * (1 - (10M / 100M))) + ((2 * 300) * (1 - (10M / 100M)));

            //Act

            decimal actual = order.GetOrderTotal();

            //Assert
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void OrderDiscountInOrderLineCanBeZero()
        {
            //Arrange
            string shippingName = "shippingName";
            string shippingCity = "shippingCity";
            string shippingZipCode = "shippingZipCode";
            string shippingAddress = "shippingAddress";

            Customer customer = new Customer();
            customer.GenerateNewIdentity();

            Order order = OrderFactory.CreateOrder(customer, shippingName, shippingCity, shippingAddress, shippingZipCode);

            order.AddNewOrderLine(Guid.NewGuid(), 1, 500, 0);
            order.AddNewOrderLine(Guid.NewGuid(), 2, 300, 0);

            decimal expected = ((1 * 500) * (1 - (0M / 100M))) + ((2 * 300) * (1 - (0M / 100M)));

            //Act
            decimal actual = order.GetOrderTotal();

            //Assert
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void OrderDiscountLessThanZeroIsEqualToZeroDiscount()
        {
            //Arrange
            string shippingName = "shippingName";
            string shippingCity = "shippingCity";
            string shippingZipCode = "shippingZipCode";
            string shippingAddress = "shippingAddress";

            Customer customer = new Customer();
            customer.GenerateNewIdentity();

            Order order = OrderFactory.CreateOrder(customer, shippingName, shippingCity, shippingAddress, shippingZipCode);

            order.AddNewOrderLine(Guid.NewGuid(), 1, 500, -10);
            order.AddNewOrderLine(Guid.NewGuid(), 2, 300, -10);

            decimal expected = ((1 * 500) * (1 - (0M / 100M))) + ((2 * 300) * (1 - (0M / 100M)));

            //Act
            decimal actual = order.GetOrderTotal();

            //Assert
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void OrderDiscountGreatherThan100IsEqualTo100Discount()
        {
            //Arrange
            string shippingName = "shippingName";
            string shippingCity = "shippingCity";
            string shippingZipCode = "shippingZipCode";
            string shippingAddress = "shippingAddress";

            Customer customer = new Customer();
            customer.GenerateNewIdentity();

            Order order = OrderFactory.CreateOrder(customer, shippingName, shippingCity, shippingAddress, shippingZipCode);

            order.AddNewOrderLine(Guid.NewGuid(), 1, 500, 101);
            order.AddNewOrderLine(Guid.NewGuid(), 2, 300, 101);

            decimal expected = ((1 * 500) * (1 - (100M / 100M))) + ((2 * 300) * (1 - (100M / 100M)));

            //Act
            decimal actual = order.GetOrderTotal();

            //Assert
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void OrderFactoryCreateValidOrder()
        {
            //Arrange

            string shippingName = "shippingName";
            string shippingCity = "shippingCity";
            string shippingZipCode = "shippingZipCode";
            string shippingAddress = "shippingAddress";

            Customer customer = new Customer();
            customer.GenerateNewIdentity();

            //Act
            Order order = OrderFactory.CreateOrder(customer, shippingName, shippingCity, shippingAddress, shippingZipCode);
            var validationContext = new ValidationContext(order, null, null);
            var validationResult = order.Validate(validationContext);

            //Assert
            ShippingInfo shippingInfo = new ShippingInfo(shippingName, shippingAddress, shippingCity, shippingZipCode);

            Assert.Equal(shippingInfo, order.ShippingInformation);
            Assert.Equal(order.Customer, customer);
            Assert.Equal(order.CustomerId, customer.Id);
            Assert.False(order.IsDelivered);
            Assert.Null(order.DeliveryDate);
            Assert.True(order.OrderDate != default(DateTime));
            Assert.False(validationResult.Any());
        }
        [Fact]
        public void IsCreditValidForOrderReturnTrueIfTotalOrderIsLessThanCustomerCredit()
        {
            //Arrange
            string shippingName = "shippingName";
            string shippingCity = "shippingCity";
            string shippingZipCode = "shippingZipCode";
            string shippingAddress = "shippingAddress";

            Country country = new Country("Spain", "es-ES");
            country.GenerateNewIdentity();

            var customer = CustomerFactory.CreateCustomer("jhon", "el rojo", "+3422", "company", country, new Address("city", "zipCode", "address line1", "addres line2"));


            //Act
            Order order = OrderFactory.CreateOrder(customer, shippingName, shippingCity, shippingAddress, shippingZipCode);
            order.AddNewOrderLine(Guid.NewGuid(), 1, 240, 0); // this is less that 1000 ( default customer credit )

            //assert
            var result = order.IsCreditValidForOrder();

            //Assert
            Assert.True(result);


        }
        [Fact]
        public void IsCreditValidForOrderReturnFalseIfTotalOrderIsGreaterThanCustomerCredit()
        {
            //Arrange
            string shippingName = "shippingName";
            string shippingCity = "shippingCity";
            string shippingZipCode = "shippingZipCode";
            string shippingAddress = "shippingAddress";

            var country = new Country("spain", "es-ES");
            country.GenerateNewIdentity();

            var customer = CustomerFactory.CreateCustomer("jhon", "el rojo", "+3422", "company", country, new Address("city", "zipCode", "address line1", "addres line2"));

            //Act
            Order order = OrderFactory.CreateOrder(customer, shippingName, shippingCity, shippingAddress, shippingZipCode);
            order.AddNewOrderLine(Guid.NewGuid(), 100, 240, 0); // this is greater that 1000 ( default customer credit )

            //assert
            var result = order.IsCreditValidForOrder();

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void OrderNumberIsComposedWithOrderDateAndSequenceOrderNumber()
        {
            //Arrange

            string shippingName = "shippingName";
            string shippingCity = "shippingCity";
            string shippingZipCode = "shippingZipCode";
            string shippingAddress = "shippingAddress";

            Customer customer = new Customer();
            customer.GenerateNewIdentity();

            Order order = OrderFactory.CreateOrder(customer, shippingName, shippingCity, shippingAddress, shippingZipCode);

            //Act
            string expected = string.Format("{0}/{1}-{2}", order.OrderDate.Year, order.OrderDate.Month, order.SequenceNumberOrder);
            string result = order.OrderNumber;

            //Assert
            Assert.Equal(expected, result);
        }
    }
}
