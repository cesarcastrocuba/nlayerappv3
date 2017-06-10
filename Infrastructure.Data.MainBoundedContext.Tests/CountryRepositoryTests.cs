namespace NLayerApp.Infrastructure.Data.MainBoundedContext.Tests
{
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using NLayerApp.Infrastructure.Data.MainBoundedContext.ERPModule.Repositories;
    using System;
    using System.Linq;
    using Xunit;

    [Collection("Our Test Collection #1")]
    public class CountryRepositoryTests : IClassFixture<MainBCUnitOfWorkInitializer>
    {
        protected MainBCUnitOfWorkInitializer fixture;
        public CountryRepositoryTests(MainBCUnitOfWorkInitializer fixture
            )
        {
            this.fixture = fixture;
        }

        [Fact]
        public void CountryRepositoryGetMethodReturnMaterializedEntityById()
        {
            //Arrange
            var countryRepository = new CountryRepository(fixture.unitOfWork, fixture.countryLogger);
            var countryId = new Guid("32BB805F-40A4-4C37-AA96-B7945C8C385C");

            //Act
            var country = countryRepository.Get(countryId);
            
            //Assert
            Assert.NotNull(country);
            Assert.True(country.Id == countryId);
        }

        [Fact]
        public void CountryRepositoryGetMethodReturnNullWhenIdIsEmpty()
        {
            //Arrange
            var countryRepository = new CountryRepository(fixture.unitOfWork, fixture.countryLogger);

            //Act
            var country = countryRepository.Get(Guid.Empty);

            //Assert
            Assert.Null(country);
        }

        [Fact]
        public void CountryRepositoryAddNewItemSaveItem()
        {
            //Arrange
            var countryRepository = new CountryRepository(fixture.unitOfWork, fixture.countryLogger);

            var country = new Country("France", "fr-FR");
            country.GenerateNewIdentity();

            //Act
            countryRepository.Add(country);
            fixture.unitOfWork.Commit();
        }

        [Fact]
        public void CountryRepositoryGetAllReturnMaterializedAllItems()
        {
            //Arrange
            var countryRepository = new CountryRepository(fixture.unitOfWork, fixture.countryLogger);

            //Act
            var allItems = countryRepository.GetAll();

            //Assert
            Assert.NotNull(allItems);
            Assert.True(allItems.Any());
        }

        [Fact]
        public void CountryRepositoryAllMatchingMethodReturnEntitiesWithSatisfiedCriteria()
        {
            //Arrange
            var countryRepository = new CountryRepository(fixture.unitOfWork, fixture.countryLogger);

            string textToFind = "ain";
            var spec = CountrySpecifications.CountryFullText(textToFind);

            //Act
            var result = countryRepository.AllMatching(spec);

            //Assert
            Assert.NotNull(result.All(c=>c.CountryISOCode.Contains(textToFind) || c.CountryName.Contains(textToFind)));

        }

        [Fact]
        public void CountryRepositoryFilterMethodReturnEntitisWithSatisfiedFilter()
        {
             //Arrange
            var countryRepository = new CountryRepository(fixture.unitOfWork, fixture.countryLogger);

            //Act
            var result =countryRepository.GetFiltered(c=>c.CountryName.Contains("EU"));

            //Assert
            Assert.NotNull(result);
            Assert.True(result.All(c=>c.CountryName.Contains("EU")));
        }

        [Fact]
        public void CountryRepositoryPagedMethodReturnEntitiesInPageFashion()
        {
            //Arrange
            var countryRepository = new CountryRepository(fixture.unitOfWork, fixture.countryLogger);

            //Act
            var pageI = countryRepository.GetPaged(0, 1, b => b.Id, false);
            var pageII = countryRepository.GetPaged(1, 1, b => b.Id, false);

            //Assert
            Assert.NotNull(pageI);
            Assert.True(pageI.Count() == 1);

            Assert.NotNull(pageII);
            Assert.True(pageII.Count() == 1);

            Assert.False(pageI.Intersect(pageII).Any());
        }
        [Fact]
        public void CountryRepositoryRemoveItemDeleteIt()
        {
            //Arrange
            var countryRepository = new CountryRepository(fixture.unitOfWork, fixture.countryLogger);

            var country = new Country("England", "en-EN");
            country.GenerateNewIdentity();
            
            countryRepository.Add(country);
            countryRepository.UnitOfWork.Commit();

            //Act
            countryRepository.Remove(country);
            fixture.unitOfWork.Commit();
        }
    }
}
