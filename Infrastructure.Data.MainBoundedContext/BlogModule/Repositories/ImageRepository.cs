namespace NLayerApp.Infrastructure.Data.MainBoundedContext.BlogModule.Repositories
{
    using NLayerApp.Domain.MainBoundedContext.Aggregates.ImageAgg;
    using NLayerApp.Infrastructure.Data.MainBoundedContext.UnitOfWork;
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
