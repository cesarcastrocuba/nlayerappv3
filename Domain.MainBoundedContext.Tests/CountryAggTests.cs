

namespace NLayerApp.Domain.MainBoundedContext.Tests
{
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using System;
    using Xunit;

    public class CountryAggTests
    {
        [Fact]
        public void CannotCreateCountryWithNullName()
        {
            //Arrange and Act
            Exception ex = Assert.Throws<ArgumentNullException>(() => new Country(null, "es-ES"));
            Assert.IsType(typeof(ArgumentNullException), ex);
        }
        [Fact]
        public void CannotCreateCountryWithNullISOCode()
        {
            //Arrange and Act
            Exception ex = Assert.Throws<ArgumentNullException>(() => new Country("spain", null));
            Assert.IsType(typeof(ArgumentNullException), ex);
        }
        [Fact]
        public void CannotCreateCountryWithEmptyName()
        {
            //Arrange and Act
            Exception ex = Assert.Throws<ArgumentNullException>(() => new Country(string.Empty, "es-ES"));
            Assert.IsType(typeof(ArgumentNullException), ex);
        }
        [Fact]
        public void CannotCreateCountryWithEmptyISOCode()
        {
            //Arrange and Act
            Exception ex = Assert.Throws<ArgumentNullException>(() => new Country("spain", string.Empty));
            Assert.IsType(typeof(ArgumentNullException), ex);
        }
        [Fact]
        public void CannotCreateCountryWithWhitespaceName()
        {
            //Arrange and Act
            Exception ex = Assert.Throws<ArgumentNullException>(() => new Country(" ", "es-ES"));
            Assert.IsType(typeof(ArgumentNullException), ex);
        }
        [Fact]
        public void CannotCreateCountryWithWhitespaceISOCode()
        {
            //Arrange and Act
            Exception ex = Assert.Throws<ArgumentNullException>(() => new Country("spain", " "));
            Assert.IsType(typeof(ArgumentNullException), ex);
        }
    }
}
