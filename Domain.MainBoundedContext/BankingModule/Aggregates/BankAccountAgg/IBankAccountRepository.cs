namespace NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg
{
    using NLayerApp.Domain.Seedwork;

    /// <summary>
    /// Base contract for bank account repository
    /// </summary>
    public interface IBankAccountRepository
        :IRepository<BankAccount>
    {
    }
}
