using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg;
using NLayerApp.Domain.Seedwork.Specification;
using NLayerApp.Infrastructure.Crosscutting.Localization;
using NLayerApp.Infrastructure.Crosscutting.NetFramework.Localization;
using System;
using Xunit;

namespace Domain.MainBoundedContext.Tests
{
    public class OrderSpecificationsTests
    {
        public OrderSpecificationsTests()
        {
            //Localization
            LocalizationFactory.SetCurrent(new ResourcesManagerFactory());
        }

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
            Exception ex = Assert.Throws<InvalidOperationException>(() => { var spec = OrdersSpecifications.OrdersByNumber(orderNumber); });
            Assert.IsType<InvalidOperationException>(ex);
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
