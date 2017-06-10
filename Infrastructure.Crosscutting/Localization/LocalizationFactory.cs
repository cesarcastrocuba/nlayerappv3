namespace NLayerApp.Infrastructure.Crosscutting.Localization
{
    public static class LocalizationFactory
    {
        #region Members

        static ILocalizationFactory _currentLocalResourcesFactory = null;

        #endregion

        #region Public Methods        
        public static void SetCurrent(ILocalizationFactory currentLocalResourcesFactory)
        {
            _currentLocalResourcesFactory = currentLocalResourcesFactory;
        }
        public static ILocalization CreateLocalResources()
        {
            return (_currentLocalResourcesFactory != null) ? _currentLocalResourcesFactory.Create() : null;
        }

        #endregion
    }
}
