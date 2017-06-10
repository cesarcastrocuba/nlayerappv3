namespace NLayerApp.Infrastructure.Crosscutting.NetFramework.Localization
{
    using NLayerApp.Infrastructure.Crosscutting.Localization;
    using NLayerApp.Infrastructure.Crosscutting.NetFramework.Resources;
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Resources;
    public class ResourcesManager : ILocalization
    {
        #region Members
        ResourceManager _ResourceManager;
        #endregion

        #region Properties
        public ResourceManager ResourceManager
        {
            get { return _ResourceManager; }
        }
        #endregion

        #region Constructor
        public ResourcesManager()
        {
            _ResourceManager = new ResourceManager(typeof(Messages));
        }
        #endregion

        #region Private Methods
        private string GetKeyFromEnum<T>(T key) where T : struct, IConvertible
        {
            if (!typeof(T).GetTypeInfo().IsEnum)
            {
                throw new ArgumentException(LocalizationFactory.CreateLocalResources().GetStringResource(LocalizationKeys.Infrastructure.exception_InvalidEnumeratedType));
            }

            return string.Format("{0}_{1}", typeof(T).Name, key.ToString());
        }
        #endregion

        #region Public Methods
        public string GetStringResource(string key)
        {
            return _ResourceManager.GetString(key);
        }

        public string GetStringResource(string key, CultureInfo culture)
        {
            return _ResourceManager.GetString(key, culture);
        }

        public string GetStringResource<T>(T key) where T : struct, IConvertible
        {
            return _ResourceManager.GetString(GetKeyFromEnum<T>(key));
        }

        public string GetStringResource<T>(T key, CultureInfo culture) where T : struct, IConvertible
        {
            return _ResourceManager.GetString(GetKeyFromEnum<T>(key), culture);
        }
        #endregion
    }
}
