
namespace NLayerApp.Infrastructure.Data.MainBoundedContext.ERPModule.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using NLayerApp.Infrastructure.Data.MainBoundedContext.UnitOfWork;
    using NLayerApp.Infrastructure.Data.Seedwork;
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;


    /// <summary>
    /// The customer repository implementation
    /// </summary>
    public class CustomerRepository
        : Repository<Customer>, ICustomerRepository
    {

        #region Constructor

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="unitOfWork">Associated unit of work</param>
        /// <param name="logger">Logger</param>
        public CustomerRepository(MainBCUnitOfWork unitOfWork,
            ILogger<Repository<Customer>> logger)
            : base(unitOfWork,logger)
        {
        }

        #endregion

        #region ICustomerRepository Members
        public override Customer Get(object id)
        {
            if (id != null && id is Guid && (Guid)id != Guid.Empty)
            {
                var currentUnitOfWork = this.UnitOfWork as MainBCUnitOfWork;

                var set = currentUnitOfWork.CreateSet<Customer>();

                return set.Include(c => c.Picture)
                          .Where(c => c.Id == (Guid)id)
                          .SingleOrDefault();
            }
            else
                return null;
        }
        public override void Merge(Customer persisted, Customer current)
        {
            //merge customer and customer picture
            var currentUnitOfWork = this.UnitOfWork as MainBCUnitOfWork;

            currentUnitOfWork.ApplyCurrentValues(persisted, current);
            currentUnitOfWork.ApplyCurrentValues(persisted.Picture, current.Picture);
        }

        public IEnumerable<Customer> GetEnabled(int pageIndex, int pageCount)
        {
            var currentUnitOfWork = this.UnitOfWork as MainBCUnitOfWork;

            return currentUnitOfWork.Customers
                                     .Where(c=>c.IsEnabled == true)
                                     .OrderBy(c => c.FullName)
                                     .Skip(pageIndex * pageCount)
                                     .Take(pageCount);
        }

        #endregion
    }
}
