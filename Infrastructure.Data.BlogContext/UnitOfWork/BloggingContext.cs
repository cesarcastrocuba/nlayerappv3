namespace NLayerApp.Infrastructure.Data.BlogBoundedContext.UnitOfWork
{
    using Microsoft.EntityFrameworkCore;
    using NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.BlogAgg;
    using NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.ImageAgg;
    using NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.PostAgg;
    using NLayerApp.Infrastructure.Data.Seedwork.UnitOfWork;

    /// <summary>
    /// The database context for the blog application.
    /// </summary>
    public class BloggingContext : BaseContext
    {
        #region DBContext Override Methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "Blogging");
        }
        #endregion

        #region DBSets
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        #endregion

        #region Constructors
        public BloggingContext()
        {
        }
        public BloggingContext(DbContextOptions<BloggingContext> options)
            : base(options)
        {
        }

        #endregion

    }
}
