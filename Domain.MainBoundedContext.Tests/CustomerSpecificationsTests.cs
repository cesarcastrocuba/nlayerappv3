using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
using NLayerApp.Domain.Seedwork.Specification;
using Xunit;

namespace Domain.MainBoundedContext.Tests
{
    public class CustomerSpecificationsTests
    {
        [Fact]
        public void CustomerFullTextEmptyTextReturnDirectSpecification()
        {
            //Arrange 
            ISpecification<Customer> spec = null;

            //Act
            spec = CustomerSpecifications.CustomerFullText(string.Empty);

            //Assert
            Assert.NotNull(spec);
            Assert.IsType<DirectSpecification<Customer>>(spec);
        }
        [Fact]
        public void CustomerFullTextNullTextReturnDirectSpecification()
        {
            //Arrange 
            ISpecification<Customer> spec = null;

            //Act
            spec = CustomerSpecifications.CustomerFullText(null);

            //Assert
            Assert.NotNull(spec);
            Assert.IsType<DirectSpecification<Customer>>(spec);
        }
        [Fact]
        public void CustomerFullTextNonEmptyTextReturnAndSpecification()
        {
            //Arrange 
            ISpecification<Customer> spec = null;

            //Act
            spec = CustomerSpecifications.CustomerFullText("Unai");

            //Assert
            Assert.NotNull(spec);
            Assert.IsType<AndSpecification<Customer>>(spec);
        }
        [Fact]
        public void CustomerEnabledCustomersSpecificationReturnDirectSpecification()
        {
            //Arrange 
            ISpecification<Customer> spec = null;

            //Act
            spec = CustomerSpecifications.EnabledCustomers();

            //Assert
            Assert.NotNull(spec);
            Assert.IsType<DirectSpecification<Customer>>(spec);
        }
    }
}
