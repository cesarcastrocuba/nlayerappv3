namespace NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg
{
    using System;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;

    /// <summary>
    /// This is the factory for Customer creation, which means that the main purpose
    /// is to encapsulate the creation knowledge.
    /// What is created is a transient entity instance, with nothing being said about persistence as yet
    /// </summary>
    public static class CustomerFactory
    {
      
        /// <summary>
        /// Create a new transient customer
        /// </summary>
        /// <param name="firstName">The customer firstName</param>
        /// <param name="lastName">The customer lastName</param>
        /// <param name="country">The associated country to this customer</param>
        /// <returns>A valid customer</returns>
        public static Customer CreateCustomer(string firstName, string lastName, string telephone,string company,Country country, Address address)
        {
            //create new instance and set identity
            var customer = new Customer();

            customer.GenerateNewIdentity();

            //set data

            customer.FirstName = firstName;
            customer.LastName = lastName;

            customer.Company = company;
            customer.Telephone = telephone;

            //set address
            customer.Address = address;

            //customer is enabled by default
            customer.Enable();

            //TODO: By default this is the limit for customer credit, you can set this 
            //parameter customizable via configuration or other system
            customer.ChangeTheCurrentCredit(1000M);


            //set default picture
            var picture = new Picture();
            picture.ChangeCurrentIdentity(customer.Id);

            customer.ChangePicture(picture);

            //set the country for this customer
            customer.SetTheCountryForThisCustomer(country);

            return customer;
        }
    }
}
