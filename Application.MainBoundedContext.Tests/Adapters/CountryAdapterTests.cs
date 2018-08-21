namespace NLayerApp.Application.MainBoundedContext.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NLayerApp.Domain.Seedwork;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using NLayerApp.Infrastructure.Crosscutting.Adapter;

    using NLayerApp.Application.MainBoundedContext.DTO;
    using NLayerApp.Infrastructure.Crosscutting.NetFramework.Adapter;
    using NLayerApp.Application.MainBoundedContext.BankingModule;
    using NLayerApp.Application.MainBoundedContext.ERPModule;
    using Xunit;

    [Collection("Our Test Collection #2")]

    public class CountryAdapterTests 
    {
        protected TestsInitialize fixture;

        public CountryAdapterTests(TestsInitialize fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void CountryToCountryDTOAdapter()
        {
            //Arrange
            var country = new Country("Spain", "es-ES");
            country.GenerateNewIdentity();

            //Act
            ITypeAdapter adapter = TypeAdapterFactory.CreateAdapter();
            var dto = adapter.Adapt<Country, CountryDTO>(country);

            //Assert
            Assert.Equal(country.Id, dto.Id);
            Assert.Equal(country.CountryName, dto.CountryName);
            Assert.Equal(country.CountryISOCode, dto.CountryISOCode);
        }
        [Fact]
        public void CountryEnumerableToCountryDTOList()
        {
            //Arrange

            var country = new Country("Spain", "es-ES");
            country.GenerateNewIdentity();

            IEnumerable<Country> countries = new List<Country>() { country };

            //Act
            ITypeAdapter adapter = TypeAdapterFactory.CreateAdapter();
            var dtos = adapter.Adapt<IEnumerable<Country>, List<CountryDTO>>(countries);

            //Assert
            Assert.NotNull(dtos);
            Assert.True(dtos.Any());
            Assert.True(dtos.Count == 1);

            var dto = dtos[0];

            Assert.Equal(country.Id, dto.Id);
            Assert.Equal(country.CountryName, dto.CountryName);
            Assert.Equal(country.CountryISOCode, dto.CountryISOCode);
        }
    }
}
