namespace NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg
{

    using System.Collections.Generic;
    using NLayerApp.Domain.Seedwork;

    /// <summary>
    /// Customer repository contract
    /// </summary>
    public interface ICustomerRepository
        :IRepository<Customer>
    {
        IEnumerable<Customer> GetEnabled(int pageIndex, int pageCount);
    }
}
