using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
using NLayerApp.Infrastructure.Crosscutting.Localization;
using NLayerApp.Infrastructure.Crosscutting.NetFramework.Localization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace Domain.MainBoundedContext.Tests
{
    public class CustomerAggTests
    {
        public CustomerAggTests()
        {
            //Localization
            LocalizationFactory.SetCurrent(new ResourcesManagerFactory());
        }

        [Fact]        
        public void CustomerCannotAssociateTransientCountry()
        {
            //Arrange
            Country country = new Country("Spain", "es-ES");

            //Act
            Customer customer = new Customer();
            Exception ex = Assert.Throws<ArgumentException>(() => customer.SetTheCountryForThisCustomer(country));
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]        
        public void CustomerCannotAssociateNullCountry()
        {
            //Arrange
            Country country = new Country("Spain", "es-ES");

            //Act
            Customer customer = new Customer();
            Exception ex = Assert.Throws<ArgumentException>(() => customer.SetTheCountryForThisCustomer(null));
            Assert.IsType<ArgumentException>(ex);            
        }
        [Fact]
        public void CustomerSetCountryFixCountryId()
        {
            //Arrange
            Country country = new Country("Spain", "es-ES");
            country.GenerateNewIdentity();

            //Act
            Customer customer = new Customer();
            customer.SetTheCountryForThisCustomer(country);

            //Assert
            Assert.Equal(country.Id, customer.CountryId);
        }
        [Fact]
        public void CustomerDisableSetIsEnabledToFalse()
        {
            //Arrange 
            var customer = new Customer();

            //Act
            customer.Disable();

            //assert
            Assert.False(customer.IsEnabled);
        }
        [Fact]
        public void CustomerEnableSetIsEnabledToTrue()
        {
            //Arrange 
            var customer = new Customer();

            //Act
            customer.Enable();

            //assert
            Assert.True(customer.IsEnabled);
        }
        [Fact]
        public void CustomerFactoryWithCountryEntityCreateValidCustomer()
        {
            //Arrange
            string lastName = "El rojo";
            string firstName = "Jhon";
            string telephone = "+34111111";
            string company = "company name";

            Country country = new Country("Spain", "es-ES");
            country.GenerateNewIdentity();

            //Act
            var customer = CustomerFactory.CreateCustomer(firstName, lastName, telephone, company, country, new Address("city", "zipcode", "AddressLine1", "AddressLine2"));
            var validationContext = new ValidationContext(customer, null, null);
            var validationResults = customer.Validate(validationContext);

            //Assert
            Assert.Equal(customer.LastName, lastName);
            Assert.Equal(customer.FirstName, firstName);
            Assert.Equal(customer.Country, country);
            Assert.Equal(customer.CountryId, country.Id);
            Assert.True(customer.IsEnabled);
            Assert.Equal(customer.Company, company);
            Assert.Equal(customer.Telephone, telephone);
            Assert.Equal(1000M, customer.CreditLimit);

            Assert.False(validationResults.Any());
        }
        [Fact]
        public void CustomerFactoryWithCountryIdEntityCreateValidCustomer()
        {
            //Arrange
            string lastName = "El rojo";
            string firstName = "Jhon";
            string telephone = "+34111111";
            string company = "company name";


            Country country = new Country("Spain", "es-ES");
            country.GenerateNewIdentity();

            //Act
            var customer = CustomerFactory.CreateCustomer(firstName, lastName, telephone, company, country, new Address("city", "zipcode", "AddressLine1", "AddressLine2"));
            var validationContext = new ValidationContext(customer, null, null);
            var validationResults = customer.Validate(validationContext);

            //Assert
            Assert.Equal(customer.LastName, lastName);
            Assert.Equal(customer.FirstName, firstName);
            Assert.Equal(customer.CountryId, country.Id);
            Assert.True(customer.IsEnabled);
            Assert.Equal(customer.Company, company);
            Assert.Equal(customer.Telephone, telephone);
            Assert.Equal(1000M, customer.CreditLimit);

            Assert.False(validationResults.Any());
        }
    }
}
