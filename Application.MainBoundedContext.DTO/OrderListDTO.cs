namespace NLayerApp.Application.MainBoundedContext.DTO
{
    using System;

    /// <summary>
    /// This is the data transfer object for
    /// Order entitiy.The name
    /// of properties for this type
    /// is based on conventions of many mappers
    /// to simplificate the mapping process.
    /// </summary>
    public class OrderListDTO
    {
        /// <summary>
        /// Get or set the order identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or set the order number
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// Get or set the order date
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Get or set the delivery date
        /// </summary>
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// Get or set the total order
        /// </summary>
        public decimal TotalOrder { get; set; }

        /// <summary>
        /// Get or set asociated customer identifier
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Get or set the customer fullname
        /// </summary>
        public string CustomerFullName { get; set; }

        /// <summary>
        /// Shipping information name
        /// </summary>
        public string ShippingName { get; set; }

        /// <summary>
        /// Get or set shipping information address
        /// </summary>
        public string ShippingAddress { get; set; }

        /// <summary>
        /// Get or set shipping information  city
        /// </summary> 
        public string ShippingCity { get; set; }

        /// <summary>
        /// Get or set shipping information zip code
        /// </summary>
        public string ShippingZipCode { get; set; }
    }
}
