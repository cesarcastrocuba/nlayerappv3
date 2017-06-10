namespace NLayerApp.Infrastructure.Crosscutting.Localization
{
    using System;
    using System.Globalization;
    public interface ILocalization
    {
        string GetStringResource(string key);
        string GetStringResource(string key, CultureInfo culture);
        string GetStringResource<T>(T key) where T : struct, IConvertible;
        string GetStringResource<T>(T key, CultureInfo culture) where T : struct, IConvertible;
    }
}
