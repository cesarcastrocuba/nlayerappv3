namespace NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg
{
    using NLayerApp.Domain.Seedwork;

    /// <summary>
    /// Base contract for product repository
    /// </summary>
    public interface  IProductRepository
        :IRepository<Product>
    {
    }
}
