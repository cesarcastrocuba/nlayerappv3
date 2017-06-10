namespace NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using NLayerApp.Domain.Seedwork;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using NLayerApp.Infrastructure.Crosscutting.Localization;

    /// <summary>
    /// Aggregate root for Customer Aggregate.
    /// </summary>
    public class Customer
        :EntityGuid,IValidatableObject
    {

        #region Members

        bool _IsEnabled;
        ILocalization messages;
        #endregion

        #region Properties

        
        /// <summary>
        /// Get or set the Given name of this customer
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Get or set the surname of this customer
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Get or set the full name of this customer
        /// </summary>
        public string FullName
        {
            get
            {
                return string.Format("{0}, {1}", this.LastName, this.FirstName);
            }
            set { } 

        }

        /// <summary>
        /// Get or set the telephone 
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// Get or set the company name
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Get or set the address of this customer
        /// </summary>
        public virtual Address Address { get; set; }

        /// <summary>
        /// Get or set the current credit limit for this customer
        /// </summary>
        public decimal CreditLimit { get; private set; }

        /// <summary>
        /// Get or set if this customer is enabled
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _IsEnabled;
            }
            private set
            {
                _IsEnabled = value;
            }
        }


        /// <summary>
        /// Get or set associated country identifier
        /// </summary>
        public Guid CountryId { get; private set; }

        /// <summary>
        /// Get the current country for this customer
        /// </summary>
        public virtual Country Country { get; private set; }

        /// <summary>
        /// Get or set associated photo for this customer
        /// </summary>
        public virtual Picture Picture { get; private set; }

        #endregion

        #region Constructor
        public Customer()
        {
            messages = LocalizationFactory.CreateLocalResources();
        }
        #endregion
        #region Public Methods

        /// <summary>
        /// Disable customer
        /// </summary>
        public void Disable()
        {
            if ( IsEnabled)
                this._IsEnabled = false;
        }

        /// <summary>
        /// Enable customer
        /// </summary>
        public void Enable()
        {
            if( !IsEnabled)
                this._IsEnabled = true;
        }

        /// <summary>
        /// Associate existing country to this customer
        /// </summary>
        /// <param name="country"></param>
        public void SetTheCountryForThisCustomer(Country country)
        {
            if (country == null
                ||
                country.IsTransient())
            {
                throw new ArgumentException(messages.GetStringResource(LocalizationKeys.Domain.exception_CannotAssociateTransientOrNullCountry));
            }

            //fix relation
            this.CountryId = country.Id;

            this.Country = country;
        }

        /// <summary>
        /// Set the country reference for this customer
        /// </summary>
        /// <param name="countryId"></param>
        public void SetTheCountryReference(Guid countryId)
        {
            if (countryId != Guid.Empty)
            {
                //fix relation
                this.CountryId = countryId;

                this.Country = null;
            }
        }

        /// <summary>
        /// Change the customer credit limit
        /// </summary>
        /// <param name="newCredit">the new credit limit</param>
        public void ChangeTheCurrentCredit(decimal newCredit)
        {
            if ( IsEnabled )
                this.CreditLimit = newCredit;
        }

        /// <summary>
        /// change the picture for this customer
        /// </summary>
        /// <param name="picture">the new picture for this customer</param>
        public void ChangePicture(Picture picture)
        {
            if (picture != null &&
                !picture.IsTransient())
            {
                this.Picture = picture;
            }
        }

        #endregion

        #region IValidatableObject Members

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            //-->Check first name property
            if (String.IsNullOrWhiteSpace(this.FirstName))
            {
                validationResults.Add(new ValidationResult(messages.GetStringResource(LocalizationKeys.Domain.validation_CustomerFirstNameCannotBeNull), 
                                                           new string[] { "FirstName" }));
            }

            //-->Check last name property
            if (String.IsNullOrWhiteSpace(this.LastName))
            {
                validationResults.Add(new ValidationResult(messages.GetStringResource(LocalizationKeys.Domain.validation_CustomerLastNameCannotBeBull),
                                                           new string[] { "LastName" }));
            }

            //-->Check Country identifier
            if (this.CountryId == Guid.Empty)
                validationResults.Add(new ValidationResult(messages.GetStringResource(LocalizationKeys.Domain.validation_CustomerCountryIdCannotBeEmpty), 
                                                          new string[] { "CountryId" }));


            return validationResults;
        }

        #endregion
    }
}
