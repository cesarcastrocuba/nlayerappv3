using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
using NLayerApp.Domain.Seedwork.Specification;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Domain.MainBoundedContext.Tests
{
    public class CountrySpecificationsTests
    {
        [Fact]
        public void CountrySpecificationsNullTextReturnTrueSpecification()
        {
            //Arrange and Act
            var specification = CountrySpecifications.CountryFullText(null);

            //Assert
            Assert.NotNull(specification);
            Assert.IsType<TrueSpecification<Country>>(specification);
        }
        [Fact]
        public void CountrySpecificationsEmptyTextReturnTrueSpecification()
        {
            //Arrange and Act
            var specification = CountrySpecifications.CountryFullText(string.Empty);

            //Assert
            Assert.NotNull(specification);
            Assert.IsType<TrueSpecification<Country>>(specification);
        }
        [Fact]
        public void CountrySpecificationNotEmptyTextReturnCompisiteSpecification()
        {
            //Arrange and Act
            var specification = CountrySpecifications.CountryFullText("Spain");

            //Assert
            Assert.NotNull(specification);
            Assert.IsType<AndSpecification<Country>>(specification);
        }
    }
}
