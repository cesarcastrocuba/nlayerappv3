
namespace NLayerApp.Infrastructure.Crosscutting.NetFramework.Localization
{
    using NLayerApp.Infrastructure.Crosscutting.Localization;
    public class ResourcesManagerFactory : ILocalizationFactory
    {
        public ILocalization Create()
        {
            return new ResourcesManager();
        }
    }
}
