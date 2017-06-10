using NLayerApp.Infrastructure.Crosscutting.Adapter;
using NLayerApp.Infrastructure.Crosscutting.Localization;
using NLayerApp.Infrastructure.Crosscutting.NetFramework.Adapter;
using NLayerApp.Infrastructure.Crosscutting.NetFramework.Localization;
using NLayerApp.Infrastructure.Crosscutting.NetFramework.Validator;
using NLayerApp.Infrastructure.Crosscutting.Validator;


namespace NLayerApp.Domain.MainBoundedContext.Tests
{
    public class TestsInitialize
    {
        public TestsInitialize()
        {
            InitializeFactories();
        }
        private void InitializeFactories()
        {
            //Localization
            LocalizationFactory.SetCurrent(new ResourcesManagerFactory());

        }
    }
}
