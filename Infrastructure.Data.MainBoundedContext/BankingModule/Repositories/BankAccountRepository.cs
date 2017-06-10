namespace NLayerApp.Infrastructure.Data.MainBoundedContext.BankingModule.Repositories
{
    using System.Linq;
    using System.Collections.Generic;

    using NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg;
    using NLayerApp.Infrastructure.Data.Seedwork;
    using NLayerApp.Infrastructure.Data.MainBoundedContext.UnitOfWork;
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The bank account repository implementation
    /// </summary>
    public class BankAccountRepository
        :Repository<BankAccount>,IBankAccountRepository
    {
        #region Constructor

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="unitOfWork">Associated unit of work</param>
        /// <param name="logger">Logger</param>
        public BankAccountRepository(MainBCUnitOfWork unitOfWork,
            ILogger<Repository<BankAccount>> logger)
            : base(unitOfWork, logger)
        {
            
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Get all bank accounts and the customer information
        /// </summary>
        /// <returns>Enumerable collection of bank accounts</returns>
        public override IEnumerable<BankAccount> GetAll()
        {
            var currentUnitOfWork = this.UnitOfWork as MainBCUnitOfWork;

            var set = currentUnitOfWork.CreateSet<BankAccount>();

            return set.Include(ba => ba.Customer)
                      .AsEnumerable();
            
        }
        #endregion
    }
}
