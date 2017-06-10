namespace NLayerApp.Application.MainBoundedContext.DTO
{
    using System;

    /// <summary>
    /// This is the data transfer object
    /// for book entity. The name
    /// of properties for this type
    /// is based on conventions of many mappers
    /// to simplificate the mapping process
    /// </summary>
    public class BookDTO
    {
        /// <summary>
        /// Get or set the identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or set the long description for this product
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Get or set the product title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Get or set the unit price for this product
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Get or set the stock items of this product
        /// </summary>
        public int AmountInStock { get; set; }

        /// <summary>
        /// Get or set the publisher of this book
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        /// Get or set related ISBN
        /// </summary>
        public string ISBN { get; set; }

    }
}
