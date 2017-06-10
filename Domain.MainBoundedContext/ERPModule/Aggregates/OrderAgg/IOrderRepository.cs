namespace NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg
{
    using NLayerApp.Domain.Seedwork;

    /// <summary>
    /// The order repository contract
    /// </summary>
    public interface IOrderRepository
        :IRepository<Order>
    {
    }
}
