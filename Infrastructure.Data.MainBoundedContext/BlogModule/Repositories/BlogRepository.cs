namespace NLayerApp.Infrastructure.Data.MainBoundedContext.BlogModule.Repositories
{
    using NLayerApp.Domain.MainBoundedContext.BlogModule.Aggregates.BlogAgg;
    using NLayerApp.Infrastructure.Data.MainBoundedContext.UnitOfWork;
    using NLayerApp.Infrastructure.Data.Seedwork;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class BlogRepository : Repository<Blog>, IBlogRepository
    {
        public BlogRepository(BloggingContext unitOfWork,
            ILogger<Repository<Blog>> logger)
            : base(unitOfWork, logger)
        {
            
        }

        #region Overrides

        public override IEnumerable<Blog> GetAll()
        {
            var currentUnitOfWork = this.UnitOfWork as BloggingContext;

            var set = currentUnitOfWork.CreateSet<Blog>();

            return set.Include("Posts.Images")
                .AsEnumerable();
        }

        public override async Task<IEnumerable<Blog>> GetAllAsync()
        {
            var currentUnitOfWork = this.UnitOfWork as BloggingContext;

            var set = currentUnitOfWork.CreateSet<Blog>();

            return await set.Include("Posts.Images").ToListAsync();
        }
        #endregion
    }
}