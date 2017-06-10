namespace NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using NLayerApp.Domain.Seedwork;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
    using NLayerApp.Infrastructure.Crosscutting.Localization;

    /// <summary>
    /// The order line representation
    /// </summary>
    public class OrderLine
        : EntityGuid, IValidatableObject
    {
        #region Members
        ILocalization messages;
        #endregion

        #region Properties

        /// <summary>
        /// Get or set the current unit price of the product in this line
        /// <remarks>
        /// The unit price cannot be less than zero
        /// </remarks>
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Get or set the amount of units in this line
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Get or set the discount associated
        /// <remarks>
        /// Discount is a value between [0-1]
        /// </remarks>
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Get the total amount of money for this line
        /// </summary>
        public decimal TotalLine
        {
            get
            {
                return (UnitPrice * Amount) * (1 - (Discount/100M));
            }
        }

        /// <summary>
        /// Related aggregate identifier
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// Get or set the product identifier
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Get or set associated product 
        /// </summary>
        public Product Product { get; private set; }

        #endregion

        #region Constructor
        public OrderLine()
        {
            messages = LocalizationFactory.CreateLocalResources();
        }
        
        #endregion
        #region Public Methods

        /// <summary>
        /// Sets a product in this order line
        /// </summary>
        /// <param name="product">The related product for this order line</param>
        public void SetProduct(Product product)
        {
            if (product == null
                ||
                product.IsTransient())
            {
                throw new ArgumentNullException(messages.GetStringResource(LocalizationKeys.Domain.exception_CannotAssociateTransientOrNullProduct));
            } 

            //fix identifiers
            this.ProductId = product.Id;
            this.Product = product;
        }

        #endregion

        #region IValidatableObject Members

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (Discount < 0 || Discount > 1)
                validationResults.Add(new ValidationResult(messages.GetStringResource(LocalizationKeys.Domain.validation_OrderLineDiscountCannotBeLessThanZeroOrGreaterThanOne),
                                                            new string[] { "Discount" }));
            if (OrderId == Guid.Empty)
                validationResults.Add(new ValidationResult(messages.GetStringResource(LocalizationKeys.Domain.validation_OrderLineOrderIdIsEmpty),
                                                           new string[] { "OrderId" }));

            if (Amount <= 0)
                validationResults.Add(new ValidationResult(messages.GetStringResource(LocalizationKeys.Domain.validation_OrderLineAmountLessThanOne),
                                                           new string[] { "Amount" }));

            if (UnitPrice < 0)
                validationResults.Add(new ValidationResult(messages.GetStringResource(LocalizationKeys.Domain.validation_OrderLineUnitPriceLessThanZero),
                                                           new string[] { "UnitPrice" }));

            if ( ProductId == Guid.Empty)
                validationResults.Add(new ValidationResult(messages.GetStringResource(LocalizationKeys.Domain.validation_OrderLineProductIdCannotBeNull),
                                                         new string[]{"ProductId"}));

            return validationResults;
        }

        #endregion
    }
}
