namespace NLayerApp.Application.MainBoundedContext.Tests
{
    using NLayerApp.Application.MainBoundedContext.DTO;
    using NLayerApp.Infrastructure.Crosscutting.Adapter;
    using NLayerApp.Infrastructure.Crosscutting.Localization;
    using NLayerApp.Infrastructure.Crosscutting.NetFramework.Adapter;
    using NLayerApp.Infrastructure.Crosscutting.NetFramework.Localization;
    using NLayerApp.Infrastructure.Crosscutting.NetFramework.Validator;
    using NLayerApp.Infrastructure.Crosscutting.Validator;
    using System;
    using Xunit;

    public class TestsInitialize : IDisposable
    {
        public TestsInitialize()
        {
            InitializeFactories();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        private void InitializeFactories()
        {
            EntityValidatorFactory.SetCurrent(new DataAnnotationsEntityValidatorFactory());

            var dto = new CountryDTO(); // this is only to force  current domain to load de .DTO assembly and all profiles

            var adapterfactory = new AutomapperTypeAdapterFactory();
            TypeAdapterFactory.SetCurrent(adapterfactory);

            //Localization
            LocalizationFactory.SetCurrent(new ResourcesManagerFactory());

        }
    }

    [CollectionDefinition("Our Test Collection #2")]
    public class Collection2 : ICollectionFixture<TestsInitialize>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

}
