namespace NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg
{
    using System;

    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg;

    /// <summary>
    /// This is the factory for Order creation, which means that the main purpose
    /// is to encapsulate the creation knowledge.
    /// What is created is a transient entity instance, with nothing being said about persistence as yet
    /// </summary>
    public static class OrderFactory
    {
        /// <summary>
        /// Create a new order
        /// </summary>
        /// <param name="customer">Associated customer</param>
        /// <param name="shippingName">The order shipping name</param>
        /// <param name="shippingCity">The order shipping city</param>
        /// <param name="shippingAddress">The order shipping address</param>
        /// <param name="shippingZipCode">The order shipping zip cocde</param>
        /// <returns>Associated order</returns>
        public static Order CreateOrder(Customer customer,string shippingName,string shippingCity,string shippingAddress,string shippingZipCode)
        {
            //create the order
            var order = new Order();

            //create shipping
            var shipping = new ShippingInfo(shippingName, shippingAddress, shippingCity, shippingZipCode);
            
            //set default values
            order.OrderDate = DateTime.UtcNow;

            order.DeliveryDate = null;

            order.ShippingInformation = shipping;

            //set customer information
            order.SetTheCustomerForThisOrder(customer);

            //set identity
            order.GenerateNewIdentity();

            return order;
        }
    }
}
