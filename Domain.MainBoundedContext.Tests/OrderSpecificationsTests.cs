namespace NLayerApp.Domain.MainBoundedContext.Tests
{
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg;
    using NLayerApp.Domain.Seedwork.Specification;
    using System;
    using Xunit;

    public class OrderSpecificationsTests : TestsInitialize
    {
        [Fact]
        public void OrderFromDateRangeNullDatesReturnTrueSpecification()
        {
            //Arrange
            DateTime? start = null;
            DateTime? end = null;

            //Act
            var spec = OrdersSpecifications.OrderFromDateRange(start, end);

            //Assert
            Assert.NotNull(spec);
            Assert.IsType<TrueSpecification<Order>>(spec);
        }
        [Fact]
        public void OrderFromDateRangeNullStartDateReturnDirectSpecification()
        {
            //Arrange
            DateTime? start = null;
            DateTime? end = DateTime.Now;

            //Act
            var spec = OrdersSpecifications.OrderFromDateRange(start, end);

            //Assert
            Assert.NotNull(spec);
            Assert.IsType<AndSpecification<Order>>(spec);
        }
        [Fact]
        public void OrderFromDateRangeNullEndDateReturnDirectSpecification()
        {
            //Arrange
            DateTime? start = DateTime.Now;
            DateTime? end = null;

            //Act
            var spec = OrdersSpecifications.OrderFromDateRange(start, end);

            //Assert
            Assert.NotNull(spec);
            Assert.IsType<AndSpecification<Order>>(spec);
        }
        [Fact]
        public void OrderByNumberThrowInvalidOperationExceptionWhenOrderNumberPatternIsIncorrect()
        {
            //Arrange

            string orderNumber = "222"; //THIS IS AN INVALID ORDER NUMBER

            //Act
            Exception ex = Assert.Throws<InvalidOperationException>(() => OrdersSpecifications.OrdersByNumber(orderNumber));
            Assert.IsType(typeof(InvalidOperationException), ex);
        }
        [Fact]
        public void OrderByNumberReturnDirectSpecificationWhenPatternIsOk()
        {
            //Arrange

            string orderNumber = "2011/12-1212"; //THIS IS AN INVALID ORDER NUMBER

            //Act
            var spec = OrdersSpecifications.OrdersByNumber(orderNumber);

            //Assert
            Assert.IsType<DirectSpecification<Order>>(spec);
        }
    }
}
