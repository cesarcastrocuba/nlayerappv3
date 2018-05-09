namespace NLayerApp.Infrastructure.Data.BlogBoundedContext.BlogModule.Repositories
{
    using NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.ImageAgg;
    using NLayerApp.Infrastructure.Data.BlogBoundedContext.UnitOfWork;
    using NLayerApp.Infrastructure.Data.Seedwork;
    using Microsoft.Extensions.Logging;
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        public ImageRepository(BloggingContext unitOfWork,
            ILogger<Repository<Image>> logger)
            : base(unitOfWork, logger)
        {

        }
    }
}
