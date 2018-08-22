using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Domain.MainBoundedContext.Tests
{
    public class CountryAggTests
    {
        [Fact]
        public void CannotCreateCountryWithNullName()
        {
            //Arrange and Act
            Exception ex = Assert.Throws<ArgumentNullException>(() => { Country country = new Country(null, "es-ES"); });
            Assert.IsType<ArgumentNullException>(ex);
        }
        [Fact]
        public void CannotCreateCountryWithNullISOCode()
        {
            //Arrange and Act            
            Exception ex = Assert.Throws<ArgumentNullException>(() => { Country country = new Country("spain", null); });
            Assert.IsType<ArgumentNullException>(ex);
        }
        [Fact]
        public void CannotCreateCountryWithEmptyName()
        {
            //Arrange and Act            
            Exception ex = Assert.Throws<ArgumentNullException>(() => { Country country = new Country(string.Empty, "es-ES"); });
            Assert.IsType<ArgumentNullException>(ex);
        }
        [Fact]
        public void CannotCreateCountryWithEmptyISOCode()
        {
            //Arrange and Act            
            Exception ex = Assert.Throws<ArgumentNullException>(() => { Country country = new Country("spain", string.Empty); });
            Assert.IsType<ArgumentNullException>(ex);
        }
        [Fact]        
        public void CannotCreateCountryWithWhitespaceName()
        {
            //Arrange and Act            
            Exception ex = Assert.Throws<ArgumentNullException>(() => { Country country = new Country(" ", "es-ES"); });
            Assert.IsType<ArgumentNullException>(ex);
        }
        [Fact]        
        public void CannotCreateCountryWithWhitespaceISOCode()
        {
            //Arrange and Act            
            Exception ex = Assert.Throws<ArgumentNullException>(() => { Country country = new Country("spain", " "); });
            Assert.IsType<ArgumentNullException>(ex);
        }
    }
}
