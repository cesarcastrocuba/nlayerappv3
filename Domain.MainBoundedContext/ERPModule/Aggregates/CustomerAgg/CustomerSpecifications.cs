namespace NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg
{

    using NLayerApp.Domain.Seedwork.Specification;
    using System;

    /// <summary>
    /// A list of customer specifications. You can learn
    /// about Specifications, Enhanced Query Objects or repository methods
    /// reading our Architecture guide and checking the DesignNotes.txt in Domain.Seedwork project
    /// </summary>
    public static class CustomerSpecifications
    {
        /// <summary>
        /// Enabled customers specification
        /// </summary>
        /// <returns>Asociated specification for this criteria</returns>
        public static Specification<Customer> EnabledCustomers()
        {
            return  new DirectSpecification<Customer>(c => c.IsEnabled);
        }

        /// <summary>
        /// Customer with firstName or LastName equal to <paramref name="text"/>
        /// </summary>
        /// <param name="text">The firstName or lastName to find</param>
        /// <returns>Associated specification for this creterion</returns>
        public static Specification<Customer> CustomerFullText(string text)
        {
            Specification<Customer> specification = new DirectSpecification<Customer>(c => c.IsEnabled);

            if (!String.IsNullOrWhiteSpace(text))
            {
                var firstNameSpec = new DirectSpecification<Customer>(c => c.FirstName.ToLower().Contains(text));
                var lastNameSpec = new DirectSpecification<Customer>(c => c.LastName.ToLower().Contains(text));

                specification &= (firstNameSpec || lastNameSpec);
            }

            return specification;
        }
    }
}
