namespace NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg
{
    using NLayerApp.Domain.Seedwork;

    /// <summary>
    /// Base contract for country repository
    /// </summary>
    public interface ICountryRepository
        :IRepository<Country>
    {
    }
}
