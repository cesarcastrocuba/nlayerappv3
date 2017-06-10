namespace NLayerApp.Application.MainBoundedContext.Tests
{
    using NLayerApp.Application.MainBoundedContext.DTO;
    using NLayerApp.Infrastructure.Crosscutting.Adapter;
    using NLayerApp.Infrastructure.Crosscutting.Localization;
    using NLayerApp.Infrastructure.Crosscutting.NetFramework.Adapter;
    using NLayerApp.Infrastructure.Crosscutting.NetFramework.Localization;
    using NLayerApp.Infrastructure.Crosscutting.NetFramework.Validator;
    using NLayerApp.Infrastructure.Crosscutting.Validator;

    public class TestsInitialize
    {
        public TestsInitialize()
        {
            InitializeFactories();
        }
        private void InitializeFactories()
        {
            EntityValidatorFactory.SetCurrent(new DataAnnotationsEntityValidatorFactory());

            var dto = new BlogDTO(); // this is only to force  current domain to load de .DTO assembly and all profiles

            var adapterfactory = new AutomapperTypeAdapterFactory();
            TypeAdapterFactory.SetCurrent(adapterfactory);

            //Localization
            LocalizationFactory.SetCurrent(new ResourcesManagerFactory());

        }
    }
    
}
