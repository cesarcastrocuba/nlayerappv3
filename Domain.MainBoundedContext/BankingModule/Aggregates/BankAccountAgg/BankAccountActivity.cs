namespace NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg
{
    using NLayerApp.Domain.Seedwork;
    using System;


    /// <summary>
    /// The bank transferLog representation
    /// </summary>
    public class BankAccountActivity
        :EntityGuid
    {
        #region Properties

        /// <summary>
        /// Get or set the bank account identifier
        /// </summary>
        public Guid BankAccountId { get; set; }

        /// <summary>
        /// The bank transferLog date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The bank transferLog amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Get or set the activity description
        /// </summary>
        public string ActivityDescription { get; set; }

        #endregion
    }
}
