namespace NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg
{
    using System;
    using NLayerApp.Domain.Seedwork.Specification;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using System.Text.RegularExpressions;
    using NLayerApp.Infrastructure.Crosscutting.Localization;

    /// <summary>
    /// The Orders specifications
    /// </summary>
    public static class OrdersSpecifications
    {
        /// <summary>
        /// Orders by Customer specification
        /// </summary>
        /// <param name="customer">The customer</param>
        /// <returns>Related specification for this criterion</returns>
        public static ISpecification<Order> OrdersByCustomer(Customer customer)
        {
            if (customer == null
                ||
                customer.IsTransient())
            {
                throw new ArgumentNullException("customer");
            }

            return new DirectSpecification<Order>(o => o.CustomerId == customer.Id);
        }
        /// <summary>
        /// Order by order number
        /// </summary>
        /// <param name="orderNumber">Find orders using the order number</param>
        /// <returns>Related specification for this criterion</returns>
        public static ISpecification<Order> OrdersByNumber(string orderNumber)
        {
            var messages = LocalizationFactory.CreateLocalResources();

            string orderNumberPattern = @"\d{4}/\d{1,2}-\d+";

            if (Regex.IsMatch(orderNumber, orderNumberPattern))
            {
                int sequenceNumber = Int32.Parse(Regex.Split(orderNumber, "-")[1]);
                return new DirectSpecification<Order>(o => o.SequenceNumberOrder == sequenceNumber);
            }
            else
                throw new InvalidOperationException(messages.GetStringResource(LocalizationKeys.Domain.exception_OrderNumberSpecificationInvalidOrderNumberPattern));
            
        }

        /// <summary>
        /// The orders in a date range
        /// </summary>
        /// <param name="startDate">The start date </param>
        /// <param name="endDate">The end date</param>
        /// <returns>Related specification for this criteria</returns>
        public static ISpecification<Order> OrderFromDateRange(DateTime? startDate, DateTime? endDate)
        {
            Specification<Order> spec = new TrueSpecification<Order>();

            if (startDate.HasValue)
                spec &= new DirectSpecification<Order>(o => o.OrderDate > (startDate ?? DateTime.MinValue));

            if (endDate.HasValue)
                spec &= new DirectSpecification<Order>(o => o.OrderDate < (endDate ?? DateTime.MaxValue));

            return spec;
        }

    }
}
