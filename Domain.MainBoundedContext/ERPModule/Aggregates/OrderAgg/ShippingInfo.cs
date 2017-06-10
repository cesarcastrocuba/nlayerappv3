namespace NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg
{
    using NLayerApp.Domain.Seedwork;

    /// <summary>
    /// Shipping information 
    /// In this specifc Domain-Model, the ShippingInfo is a Value-Object.
    /// </summary>
    public class ShippingInfo
        :ValueObject<ShippingInfo>
    {
        #region Constructor

        /// <summary>
        /// Create a new instance of shipping info, providing its values. It will be immutable.
        /// </summary>
        /// <param name="shippingName">The shipping name</param>
        /// <param name="shippingAddress">The shipping address</param>
        /// <param name="shippingCity">The shipping city</param>
        /// <param name="shippingZipCode">The shipping zip code</param>
        public ShippingInfo(string shippingName, string shippingAddress, string shippingCity, string shippingZipCode)
        {
            this.ShippingName = shippingName;
            this.ShippingAddress = shippingAddress;
            this.ShippingCity = shippingCity;
            this.ShippingZipCode = shippingZipCode;

        }

        private ShippingInfo()  //required for EF
        {
        }

        #endregion

        #region Properties

        //Sets are 'private' because this class is a Value-Object
        //Therefore, it must be immutable from its creation.

        /// <summary>
        /// Get or set the shipping name
        /// </summary>
        public string ShippingName { get; private set; }

        /// <summary>
        /// Get or set the shipping address
        /// </summary>
        public string ShippingAddress { get; private set; }

        /// <summary>
        /// Get or set the shipping city
        /// </summary>
        public string ShippingCity { get; private set; }

        /// <summary>
        /// Get or set the shipping zip code
        /// </summary>
        public string ShippingZipCode { get; private set; }

        #endregion
    }
}
