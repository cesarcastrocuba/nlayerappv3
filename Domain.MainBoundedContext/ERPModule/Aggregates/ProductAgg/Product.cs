namespace NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg
{
    using NLayerApp.Domain.Seedwork;
    using NLayerApp.Infrastructure.Crosscutting.Localization;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Product aggregate root-entity
    /// </summary>
    public abstract class Product
        :EntityGuid,IValidatableObject
    {
        
        #region Properties

        /// <summary>
        /// Get or set the long description for this product
        /// </summary>
        public string Description { get; protected set; }

        /// <summary>
        /// Get or set the product title
        /// </summary>
        public string Title { get; protected set; }

        /// <summary>
        /// Get or set the unit price for this product
        /// </summary>
        public decimal UnitPrice { get; private set; }

        /// <summary>
        /// Get or set the stock items of this product
        /// </summary>
        public int AmountInStock { get; private set; }


        #endregion
        
        #region Public Methods

        /// <summary>
        /// Change the unit price
        /// </summary>
        /// <param name="unitPrice">The new unit price</param>
        public void ChangeUnitPrice(decimal unitPrice)
        {
            this.UnitPrice = unitPrice;
        }

        /// <summary>
        /// Increment the stock of this product
        /// </summary>
        /// <param name="units">The added items to stock</param>
        public void IncrementStock(int units = 0)
        {
            this.AmountInStock += units;
        }

        #endregion

        #region IValidatableObject Members

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ILocalization messages = LocalizationFactory.CreateLocalResources();

            var validationResults = new List<ValidationResult>();
            
            if (AmountInStock < 0)
                validationResults.Add(new ValidationResult(messages.GetStringResource(LocalizationKeys.Domain.validation_ProductAmountLessThanZero), new string[] { "AmountInStock" }));
           
            if (UnitPrice < 0)
                validationResults.Add(new ValidationResult(messages.GetStringResource(LocalizationKeys.Domain.validation_ProductUnitPriceLessThanZero), new string[] { "UnitPrice" }));
            
            return validationResults;
        }

        #endregion  
    }
}
