namespace NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg
{
    using System;
    using NLayerApp.Domain.Seedwork;

    /// <summary>
    /// The country entity
    /// </summary>
    public class Country
        :EntityGuid
    {
        #region Properties

        /// <summary>
        /// Get or set the Country Name
        /// </summary>
        public string CountryName { get; private set; }

        /// <summary>
        /// Get or set the Country ISO Code
        /// </summary>
        public string CountryISOCode { get; private set; }

        #endregion

        #region Constructor

        //required by EF
        private Country() { } 

        public Country(string countryName, string countryISOCode)
        {
            if (String.IsNullOrWhiteSpace(countryName))
                throw new ArgumentNullException("countryName");

            if (String.IsNullOrWhiteSpace(countryISOCode))
                throw new ArgumentNullException("countryISOCode");

            this.CountryName = countryName;
            this.CountryISOCode = countryISOCode;
        }

        #endregion
    }
}
