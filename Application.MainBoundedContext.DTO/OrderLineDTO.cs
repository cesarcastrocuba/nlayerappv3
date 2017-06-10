namespace NLayerApp.Application.MainBoundedContext.DTO
{
    using System;

    /// <summary>
    /// This is the data transfer object for
    /// OrderLine entitiy.The name
    /// of properties for this type
    /// is based on conventions of many mappers
    /// to simplificate the mapping process.
    /// </summary>
    public class OrderLineDTO
    {
        /// <summary>
        /// Get or set the order identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or set the unit price
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Get or set the # of items
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Get or set the discount
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Get or set the total line
        /// </summary>
        public decimal TotalLine { get; set; }

        /// <summary>
        /// Get or set the associated product
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Get or set the product title
        /// </summary>
        public string ProductTitle { get; set; }

    }
}
