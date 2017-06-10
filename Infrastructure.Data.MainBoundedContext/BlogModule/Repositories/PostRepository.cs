namespace NLayerApp.Infrastructure.Data.MainBoundedContext.BlogModule.Repositories
{
    using NLayerApp.Domain.MainBoundedContext.Aggregates.PostAgg;
    using NLayerApp.Domain.Seedwork.Specification;
    using NLayerApp.Infrastructure.Data.MainBoundedContext.UnitOfWork;
    using NLayerApp.Infrastructure.Data.Seedwork;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(BloggingContext unitOfWork,
            ILogger<Repository<Post>> logger)
            : base(unitOfWork, logger)
        {
            
        }

        #region Overrides
        public override IEnumerable<Post> AllMatching(ISpecification<Post> specification)
        {
            var currentUnitOfWork = this.UnitOfWork as BloggingContext;

            var set = currentUnitOfWork.CreateSet<Post>();

            return set.Include(o => o.Images)
                      .Where(specification.SatisfiedBy())
                      .AsEnumerable();
        }
        public override async Task<IEnumerable<Post>> AllMatchingAsync(ISpecification<Post> specification)
        {
            var currentUnitOfWork = this.UnitOfWork as BloggingContext;

            var set = currentUnitOfWork.CreateSet<Post>();

            return await set.Include(o => o.Images)
                      .Where(specification.SatisfiedBy()).ToListAsync();
        }

        #endregion
    }
}