namespace NLayerApp.Infrastructure.Data.MainBoundedContext.UnitOfWork
{
    using NLayerApp.Domain.MainBoundedContext.BlogModule.Aggregates.BlogAgg;
    using NLayerApp.Domain.MainBoundedContext.Aggregates.ImageAgg;
    using NLayerApp.Domain.MainBoundedContext.Aggregates.PostAgg;
    using NLayerApp.Domain.Seedwork;
    using NLayerApp.Infrastructure.Data.Seedwork;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Security.Claims;

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
