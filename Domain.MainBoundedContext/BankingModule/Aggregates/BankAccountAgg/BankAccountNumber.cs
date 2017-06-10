namespace NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg
{
    using NLayerApp.Domain.Seedwork;

    /// <summary>
    /// The bank account number value object
    /// </summary>
    public class BankAccountNumber
        :ValueObject<BankAccountNumber>
    {
        #region Constructor

        /// <summary>
        /// Create a new instance of bank account number
        /// </summary>
        /// <param name="officeNumber">The office number</param>
        /// <param name="nationalBankCode">The national bank code</param>
        /// <param name="accountNumber">The account number</param>
        /// <param name="checkDigits">The check digits</param>
        public BankAccountNumber(string officeNumber, string nationalBankCode, string accountNumber, string checkDigits)
        {
            // you can check data here!

            OfficeNumber = officeNumber;
            NationalBankCode = nationalBankCode;
            AccountNumber = accountNumber;
            CheckDigits = checkDigits;
        }

        /// <summary>
        /// Create a new instance
        /// <remarks>
        /// This is a "requirement" for the persistence infrastructure 
        /// and proxy creation :-(
        /// </remarks>
        /// </summary>
        public BankAccountNumber() { }

        #endregion

        #region Properties

        /// <summary>
        /// The office number
        /// </summary>
        public string OfficeNumber { get; set; }

        /// <summary>
        /// The national bank code
        /// </summary>
        public string NationalBankCode { get; set; }

        /// <summary>
        /// The account number
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// The check digits
        /// </summary>
        public string CheckDigits { get; set; }

        #endregion
    }
}
