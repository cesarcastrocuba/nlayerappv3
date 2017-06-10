namespace NLayerApp.Infrastructure.Data.MainBoundedContext.ERPModule.Repositories
{
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
    using NLayerApp.Infrastructure.Data.MainBoundedContext.UnitOfWork;
    using NLayerApp.Infrastructure.Data.Seedwork;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Product repository implementation
    /// </summary>
    public class ProductRepository
        :Repository<Product>,IProductRepository
    {
        #region Constructor

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="unitOfWork">Associated unit of work</param>
        /// <param name="logger">Logger</param>
        public ProductRepository(MainBCUnitOfWork unitOfWork,
            ILogger<Repository<Product>> logger)
            : base(unitOfWork,logger)
        {
        }

        #endregion
    }
}
