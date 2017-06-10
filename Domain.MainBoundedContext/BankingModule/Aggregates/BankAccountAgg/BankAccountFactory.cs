namespace NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg
{
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;

    /// <summary>
    /// This is the factory for bank account creation, which means that the main purpose
    /// is to encapsulate the creation knowledge. Please read Design Notes for Factoris and 
    /// when you can create this..
    /// </summary>
    public static class BankAccountFactory
    {
        /// <summary>
        /// Create a new instance of bankaccount
        /// </summary>
        /// <param name="customer">The customer associated with this bank account</param>
        /// <param name="bankAccountNumber">The bank account number</param>
        /// <returns>A valid bank account</returns>
        public static BankAccount CreateBankAccount(Customer customer, BankAccountNumber bankAccountNumber)
        {
            var bankAccount = new BankAccount();

            //set the identity
            bankAccount.GenerateNewIdentity();

            //set the bank account number
            bankAccount.BankAccountNumber = bankAccountNumber;

            //set default bank account as unlocked
            bankAccount.UnLock();

            //set the customer for this bank account
            bankAccount.SetCustomerOwnerOfThisBankAccount(customer);

            return bankAccount;
        }
    }
}
