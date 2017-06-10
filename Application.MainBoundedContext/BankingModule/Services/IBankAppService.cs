namespace NLayerApp.Application.MainBoundedContext.BankingModule.Services
{
    using System;
    using System.Collections.Generic;

    using NLayerApp.Application.MainBoundedContext.DTO;

    /// <summary>
    /// This is the contract that the application will interact to perform various operations for "banking management".
    /// The responsability of this contract is oschestrate operations, check security, cache,
    /// adapt entities to DTO etc,
    /// </summary>
    public interface IBankAppService : IDisposable
    {
        /// <summary>
        /// Add new bank acccount
        /// </summary>
        /// <param name="bankAccountDTO">The bank account representation to add</param>
        /// <returns>Added bank account</returns>
        BankAccountDTO AddBankAccount(BankAccountDTO bankAccountDTO);

        /// <summary>
        /// Lock existing bank account
        /// </summary>
        /// <param name="bankAccountId">The bank account identifier</param>
        /// <returns>True if lock succed, else false</returns>
        bool LockBankAccount(Guid bankAccountId);

        /// <summary>
        /// Find the current collection of bank accounts
        /// </summary>
        /// <returns>A collection of bank accounts</returns>
        List<BankAccountDTO> FindBankAccounts();

        /// <summary>
        /// Find bank activity for a existent bank account 
        /// </summary>
        /// <param name="bankAccountId">The bank account identifier</param>
        /// <returns>A collection of bank activities</returns>
        List<BankActivityDTO> FindBankAccountActivities(Guid bankAccountId);

        /// <summary>
        /// Perform a bank transfer into accocunts
        /// </summary>
        /// <param name="fromAccount">The bank transfer source account</param>
        /// <param name="toAccount">The bank transfer target account</param>
        /// <param name="amount">The amount to transfer</param>
        void PerformBankTransfer(BankAccountDTO fromAccount, BankAccountDTO toAccount, decimal amount);

    }
}
