namespace NLayerApp.Domain.MainBoundedContext.BankingModule.Services
{
    using NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg;
    using System;
    using NLayerApp.Infrastructure.Crosscutting.Localization;

    /// <summary>
    /// Bank transfer service implementation. 
    /// </summary>
    public class BankTransferService : IBankTransferService
    {
        public void PerformTransfer(decimal amount, BankAccount originAccount, BankAccount destinationAccount)
        {
            if (originAccount != null && destinationAccount != null)
            {
                var messages = LocalizationFactory.CreateLocalResources();

                if (originAccount.BankAccountNumber == destinationAccount.BankAccountNumber) // if transfer in same bank account
                    throw new InvalidOperationException(messages.GetStringResource(LocalizationKeys.Domain.exception_CannotTransferMoneyWhenFromIsTheSameAsTo));

                // Check if customer has required credit and if the BankAccount is not locked
                if (originAccount.CanBeWithdrawed(amount))
                {
                    //Domain Logic
                    //Process: Perform transfer operations to in-memory Domain-Model objects        
                    // 1.- Charge money to origin acc
                    // 2.- Credit money to destination acc

                    //Charge money
                    originAccount.WithdrawMoney(amount, string.Format(messages.GetStringResource(LocalizationKeys.Domain.messages_TransactionFromMessage), destinationAccount.Id));

                    //Credit money
                    destinationAccount.DepositMoney(amount, string.Format(messages.GetStringResource(LocalizationKeys.Domain.messages_TransactionToMessage), originAccount.Id));
                }
                else
                    throw new InvalidOperationException(messages.GetStringResource(LocalizationKeys.Domain.exception_BankAccountCannotWithdraw));
            }
        }
    }
}
