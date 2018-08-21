namespace NLayerApp.Application.MainBoundedContext.Tests
{
    using NLayerApp.Application.MainBoundedContext.DTO;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using NLayerApp.Infrastructure.Crosscutting.Adapter;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    [Collection("Our Test Collection #2")]

    public class CustomerAdaperTests 
    {
        protected TestsInitialize fixture;

        public CustomerAdaperTests(TestsInitialize fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void CustomerToCustomerDTOAdapt()
        {
            //Arrange

            var country = new Country("Spain", "es-ES");
            country.GenerateNewIdentity();

            var address = new Address("Monforte", "27400", "AddressLine1", "AddressLine2");

            var customer = CustomerFactory.CreateCustomer("Jhon", "El rojo", "617404929", "Spirtis", country, address);
            var picture = new Picture { RawPhoto = new byte[0] { } };

            customer.ChangeTheCurrentCredit(1000M);
            customer.ChangePicture(picture);
            customer.SetTheCountryForThisCustomer(country);

            //Act

            ITypeAdapter adapter = TypeAdapterFactory.CreateAdapter();
            var dto = adapter.Adapt<Customer, CustomerDTO>(customer);

            //Assert

            Assert.Equal(customer.Id, dto.Id);
            Assert.Equal(customer.FirstName, dto.FirstName);
            Assert.Equal(customer.LastName, dto.LastName);
            Assert.Equal(customer.Company, dto.Company);
            Assert.Equal(customer.Telephone, dto.Telephone);
            Assert.Equal(customer.CreditLimit, dto.CreditLimit);

            Assert.Equal(customer.Country.CountryName, dto.CountryCountryName);
            Assert.Equal(country.Id, dto.CountryId);


            Assert.Equal(customer.Address.City, dto.AddressCity);
            Assert.Equal(customer.Address.ZipCode, dto.AddressZipCode);
            Assert.Equal(customer.Address.AddressLine1, dto.AddressAddressLine1);
            Assert.Equal(customer.Address.AddressLine2, dto.AddressAddressLine2);
        }

        [Fact]
        public void CustomerEnumerableToCustomerListDTOListAdapt()
        {
            //Arrange



            var country = new Country("Spain", "es-ES");
            country.GenerateNewIdentity();

            var address = new Address("Monforte", "27400", "AddressLine1", "AddressLine2");

            var customer = CustomerFactory.CreateCustomer("Jhon", "El rojo", "617404929", "Spirtis", country, address);
            var picture = new Picture { RawPhoto = new byte[0] { } };

            customer.ChangeTheCurrentCredit(1000M);
            customer.ChangePicture(picture);
            customer.SetTheCountryForThisCustomer(country);

            IEnumerable<Customer> customers = new List<Customer>() { customer };

            //Act
            ITypeAdapter adapter = TypeAdapterFactory.CreateAdapter();

            var dtos = adapter.Adapt<IEnumerable<Customer>, List<CustomerListDTO>>(customers);

            //Assert

            Assert.NotNull(dtos);
            Assert.True(dtos.Any());
            Assert.True(dtos.Count == 1);

            CustomerListDTO dto = dtos[0];

            Assert.Equal(customer.Id, dto.Id);
            Assert.Equal(customer.FirstName, dto.FirstName);
            Assert.Equal(customer.LastName, dto.LastName);
            Assert.Equal(customer.Company, dto.Company);
            Assert.Equal(customer.Telephone, dto.Telephone);
            Assert.Equal(customer.CreditLimit, dto.CreditLimit);
            Assert.Equal(customer.Address.City, dto.AddressCity);
            Assert.Equal(customer.Address.ZipCode, dto.AddressZipCode);
            Assert.Equal(customer.Address.AddressLine1, dto.AddressAddressLine1);
            Assert.Equal(customer.Address.AddressLine2, dto.AddressAddressLine2);


        }
    }
}
