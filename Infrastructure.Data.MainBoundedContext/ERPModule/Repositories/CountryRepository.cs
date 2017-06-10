namespace NLayerApp.Infrastructure.Data.MainBoundedContext.ERPModule.Repositories
{
    using Microsoft.Extensions.Logging;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using NLayerApp.Infrastructure.Data.MainBoundedContext.UnitOfWork;
    using NLayerApp.Infrastructure.Data.Seedwork;

    /// <summary>
    /// The country repository implementation
    /// </summary>
    public class CountryRepository
        :Repository<Country>,ICountryRepository
    {
        #region Constructor

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="unitOfWork">Associated unit of work</param>
        /// <param name="logger">Logger</param>
        public CountryRepository(MainBCUnitOfWork unitOfWork,
            ILogger<Repository<Country>> logger)
            : base(unitOfWork,logger)
        {
        }

        #endregion
    }
}
