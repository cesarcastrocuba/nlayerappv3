namespace NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg
{
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using NLayerApp.Domain.Seedwork;
    using NLayerApp.Infrastructure.Crosscutting.Localization;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The bank account representation (Domain Entity)
    /// </summary>
    public class BankAccount
        :EntityGuid,IValidatableObject
    {
        #region Members
        ILocalization messages;
        #endregion
        #region Constructor
        public BankAccount()
        {
            messages = LocalizationFactory.CreateLocalResources();
        }
        #endregion
        #region Properties

        /// <summary>
        /// Get or set the bank account number
        /// </summary>

        public BankAccountNumber BankAccountNumber { get; set; }

        /// <summary>
        /// Get the IBAN  (International Bank Account Number)
        /// <remarks>
        /// http://en.wikipedia.org/wiki/International_Bank_Account_Number
        /// Spanish format
        /// </remarks>
        /// </summary>
        public string Iban
        {
            get
            {
                if (this.BankAccountNumber != null)
                    return string.Format("ES{0} {1} {2} {0}{3}",
                                        this.BankAccountNumber.CheckDigits,
                                        this.BankAccountNumber.NationalBankCode,
                                        this.BankAccountNumber.OfficeNumber,
                                        this.BankAccountNumber.AccountNumber);
                else
                    return "No Bank Account Provided";
            }
            set
            {
            }
        }

        /// <summary>
        /// Get the current balance of this account
        /// </summary>
        public decimal Balance { get; private set; }

        /// <summary>
        /// Get the state of this account
        /// </summary>
        public bool Locked { get; private set; }

        /// <summary>
        /// Get or set the customer id associated with this bank account
        /// </summary>
        public Guid CustomerId { get; private set; }
       
        /// <summary>
        /// The related customer
        /// </summary>
        public virtual Customer Customer { get; private set; }

        HashSet<BankAccountActivity> _bankAccountActivity;

        /// <summary>
        /// Get the bank account activity into this account
        /// </summary>
        public virtual ICollection<BankAccountActivity> BankAccountActivity
        {
            get
            {
                if (_bankAccountActivity == null)
                    _bankAccountActivity = new HashSet<BankAccountActivity>();

                return _bankAccountActivity;
            }
            set
            {
                _bankAccountActivity = new HashSet<BankAccountActivity>(value);
            }
        }

       
        #endregion

        #region Public Methods

        
        /// <summary>
        /// Lock current bank account
        /// </summary>
        public void Lock()
        {
            if (!Locked)
                Locked = true;
        }

        /// <summary>
        /// Un lock current bank account
        /// </summary>
        public void UnLock()
        {
            if (Locked)
                Locked = false;
        }

        /// <summary>
        /// Deposit momey into this bank account
        /// </summary>
        /// <param name="amount">the amount of money to deposit </param>
        public void DepositMoney(decimal amount,string reason)
        {
            if (amount < 0) throw new ArgumentException(messages.GetStringResource(LocalizationKeys.Domain.exception_BankAccountInvalidWithdrawAmount));

            //DepositMoney is a term of our Ubiquitous Language. Means adding money to this account
            if (!this.Locked)
            {
                //set balance
                checked
                {
                    Balance += amount;

                    //anotate activity
                    var activity = new BankAccountActivity()
                    {
                        Date = DateTime.UtcNow,
                        Amount = amount,
                        ActivityDescription = reason,
                        BankAccountId = Id
                    };
                    activity.GenerateNewIdentity();

                    this.BankAccountActivity.Add(activity);
                }
            }
            else
                throw new InvalidOperationException(messages.GetStringResource(LocalizationKeys.Domain.exception_BankAccountCannotDeposit));
        }

        /// <summary>
        /// WithdrawMoney operation over this bankaccount
        /// </summary>
        /// <param name="amount">The amount of money for this withdraw operation</param>
        public void WithdrawMoney(decimal amount,string reason)
        {
            if ( amount < 0 )  throw new ArgumentException(messages.GetStringResource(LocalizationKeys.Domain.exception_BankAccountInvalidWithdrawAmount));

            //WithdrawMoney is a term of our Ubiquitous Language. Means deducting money to this account
            if (CanBeWithdrawed(amount))
            {
                checked
                {
                    this.Balance -= amount;

                    //anotate activity
                    var activity = new BankAccountActivity()
                    {
                        Date = DateTime.UtcNow,
                        Amount = -amount,
                        ActivityDescription = reason,
                        BankAccountId = Id
                    };
                    activity.GenerateNewIdentity();

                    this.BankAccountActivity.Add(activity);
                }
            }
            else
                throw new InvalidOperationException(messages.GetStringResource(LocalizationKeys.Domain.exception_BankAccountCannotWithdraw));
        }

        /// <summary>
        /// Check if in this bankaccount is posible withdraw <paramref name="amount"/>
        /// </summary>
        /// <param name="amount">The amount of money </param>
        /// <returns>True if is posible perform the operation, else false</returns>
        public bool CanBeWithdrawed(decimal amount)
        {
            return !Locked && (this.Balance >= amount);
        }

        /// <summary>
        /// Set customer for this bank account
        /// </summary>
        /// <param name="customer">The customer owner of this bank account</param>
        public void SetCustomerOwnerOfThisBankAccount(Customer customer)
        {
            if (customer == null
                ||
                customer.IsTransient())
            {
                throw new ArgumentException(messages.GetStringResource(LocalizationKeys.Domain.exception_CannotAssociateTransientOrNullCustomer));
            }

            //fix id and set reference
            this.CustomerId = customer.Id;
            this.Customer = customer;
        }

        /// <summary>
        /// Change current customer reference using a customer id
        /// </summary>
        /// <param name="customerId">The new customer identifier</param>
        public void ChangeCustomerOwnerReference(Guid customerId)
        {
            if (customerId != Guid.Empty)
            {
                //fix a new id 
                this.Customer = null;
                this.CustomerId = customerId;
            }
        }

        #endregion

        #region IValidatableObject Members

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (this.BankAccountNumber == null)
                validationResults.Add(new ValidationResult(messages.GetStringResource(LocalizationKeys.Domain.validation_BankAccountNumberCannotBeNull), new string[] { "BankAccountNumber" }));
            else
            {
                if (String.IsNullOrWhiteSpace(this.BankAccountNumber.AccountNumber))
                    validationResults.Add(new ValidationResult(messages.GetStringResource(LocalizationKeys.Domain.validation_BankAccountBankAccountNumberCannotBeNull), new string[] { "AccountNumber" }));

                if (String.IsNullOrWhiteSpace(this.BankAccountNumber.CheckDigits))
                    validationResults.Add(new ValidationResult(messages.GetStringResource(LocalizationKeys.Domain.validation_BankAccountBankCheckDigitsCannotBeNull), new string[] { "CheckDigits" }));

                if (String.IsNullOrWhiteSpace(this.BankAccountNumber.NationalBankCode))
                    validationResults.Add(new ValidationResult(messages.GetStringResource(LocalizationKeys.Domain.validation_BankAccountBankNationalBankCodeCannotBeNull), new string[] { "NationalBankCode" }));

                if (String.IsNullOrWhiteSpace(this.BankAccountNumber.OfficeNumber))
                    validationResults.Add(new ValidationResult(messages.GetStringResource(LocalizationKeys.Domain.validation_BankAccountBankOfficeNumberCannotBeNull), new string[] { "OfficeNumber" }));
            }

            if(CustomerId == Guid.Empty)
                validationResults.Add(new ValidationResult(messages.GetStringResource(LocalizationKeys.Domain.validation_BankAccountCustomerIdIsEmpty), new string[] { "CustomerId" }));

            return validationResults;
        }

        #endregion
    }
}
