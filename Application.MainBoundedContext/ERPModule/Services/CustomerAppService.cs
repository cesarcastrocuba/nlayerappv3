namespace NLayerApp.Application.MainBoundedContext.ERPModule.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NLayerApp.Application.MainBoundedContext.DTO;
    using NLayerApp.Application.Seedwork;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using NLayerApp.Domain.Seedwork.Specification;
    using NLayerApp.Infrastructure.Crosscutting.Validator;
    using NLayerApp.Infrastructure.Crosscutting.Localization;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The customer management service implementation.
    /// </summary>
    public class CustomerAppService
        : ICustomerAppService
    {
        #region Members

        readonly ICountryRepository _countryRepository;
        readonly ICustomerRepository _customerRepository;

        readonly ILogger _logger;
        readonly ILocalization _resources;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of Customer Management Service
        /// </summary>
        /// <param name="customerRepository">Associated CustomerRepository, intented to be resolved with DI</param>
        /// <param name="countryRepository">Associated country repository</param>
        public CustomerAppService(ICountryRepository countryRepository, //the country repository
                                  ICustomerRepository customerRepository, //the customer repository                               
                                  ILogger<CustomerAppService> logger) 
        {
            if (customerRepository == null)
                throw new ArgumentNullException("customerRepository");

            if (countryRepository == null)
                throw new ArgumentNullException("countryRepository");

            _countryRepository = countryRepository;
            _customerRepository = customerRepository;

            _logger = logger;
            _resources = LocalizationFactory.CreateLocalResources();
        }

        #endregion

        #region ICustomerAppService Members

        public CustomerDTO AddNewCustomer(CustomerDTO customerDTO)
        {
            //check preconditions
            if (customerDTO == null || customerDTO.CountryId == Guid.Empty)
                throw new ArgumentException(_resources.GetStringResource(LocalizationKeys.Application.warning_CannotAddCustomerWithEmptyInformation));

            var country = _countryRepository.Get(customerDTO.CountryId);

            if (country != null)
            {
                //Create the entity and the required associated data
                var address = new Address(customerDTO.AddressCity, customerDTO.AddressZipCode, customerDTO.AddressAddressLine1, customerDTO.AddressAddressLine2);

                var customer = CustomerFactory.CreateCustomer(customerDTO.FirstName,
                                                              customerDTO.LastName,
                                                              customerDTO.Telephone,
                                                              customerDTO.Company,
                                                              country,
                                                              address);

                //save entity
                SaveCustomer(customer);

                //return the data with id and assigned default values
                return customer.ProjectedAs<CustomerDTO>();
            }
            else
                return null;
        }

        public void UpdateCustomer(CustomerDTO customerDTO)
        {
            if (customerDTO == null || customerDTO.Id == Guid.Empty)
                throw new ArgumentException(_resources.GetStringResource(LocalizationKeys.Application.warning_CannotUpdateCustomerWithEmptyInformation));

            //get persisted item
            var persisted = _customerRepository.Get(customerDTO.Id);

            if (persisted != null) //if customer exist
            {
                //materialize from customer dto
                var current = MaterializeCustomerFromDto(customerDTO);

                //Merge changes
                _customerRepository.Merge(persisted, current);

                //commit unit of work
                _customerRepository.UnitOfWork.Commit();
            }
            else
                _logger.LogWarning(_resources.GetStringResource(LocalizationKeys.Application.warning_CannotUpdateNonExistingCustomer));
        }

        public void RemoveCustomer(Guid customerId)
        {
            var customer = _customerRepository.Get(customerId);

            if (customer != null) //if customer exist
            {
                //disable customer ( "logical delete" ) 
                customer.Disable();

                //commit changes
                _customerRepository.UnitOfWork.Commit();
            }
            else //the customer not exist, cannot remove
                _logger.LogWarning(_resources.GetStringResource(LocalizationKeys.Application.warning_CannotRemoveNonExistingCustomer));
        }

        public List<CustomerListDTO> FindCustomers(int pageIndex, int pageCount)
        {
            if (pageIndex < 0 || pageCount <= 0)
                throw new ArgumentException(_resources.GetStringResource(LocalizationKeys.Application.warning_InvalidArgumentsForFindCustomers));

            //get customers
            var customers = _customerRepository.GetEnabled(pageIndex, pageCount);

            if (customers != null
                &&
                customers.Any())
            {
                return customers.ProjectedAsCollection<CustomerListDTO>();
            }
            else
                return null;
        }
        
        public List<CustomerListDTO> FindCustomers(string text)
        {
            //get the specification

            var enabledCustomers = CustomerSpecifications.EnabledCustomers();
            var filter = CustomerSpecifications.CustomerFullText(text);

            ISpecification<Customer> spec = enabledCustomers & filter;

            //Query this criteria
            var customers = _customerRepository.AllMatching(spec);

            if (customers != null
                &&
                customers.Any())
            {
                //return adapted data
                return customers.ProjectedAsCollection<CustomerListDTO>();
            }
            else // no data..
                return null;
        }

        public CustomerDTO FindCustomer(Guid customerId)
        {
            //recover existing customer and map
            var customer = _customerRepository.Get(customerId);

            if (customer != null) //adapt
            {
                return customer.ProjectedAs<CustomerDTO>();
            }
            else
                return null;
        }

        public List<CountryDTO> FindCountries(int pageIndex, int pageCount)
        {
            if (pageIndex < 0 || pageCount <= 0)
                throw new ArgumentException(_resources.GetStringResource(LocalizationKeys.Application.warning_InvalidArgumentsForFindCountries));
            
            //recover countries
            var countries = _countryRepository.GetPaged(pageIndex, pageCount, c => c.CountryName, false);

            if (countries != null
                &&
                countries.Any())
            {
                return countries.ProjectedAsCollection<CountryDTO>();
            }
            else // no data.
                return null;

        }
        
        public List<CountryDTO> FindCountries(string text)
        {
            //get the specification
            ISpecification<Country> specification = CountrySpecifications.CountryFullText(text);

            //Query this criteria
            var countries = _countryRepository.AllMatching(specification);

            if (countries != null
                &&
                countries.Any())
            {
                return countries.ProjectedAsCollection<CountryDTO>();
            }
            else // no data
                return null;
        }

        #endregion

        #region Private Methods

        void SaveCustomer(Customer customer)
        {
            //recover validator
            var validator = EntityValidatorFactory.CreateValidator();

            if (validator.IsValid(customer)) //if customer is valid
            {
                //add the customer into the repository
                _customerRepository.Add(customer);

                //commit the unit of work
                _customerRepository.UnitOfWork.Commit();
            }
            else //customer is not valid, throw validation errors
                throw new ApplicationValidationErrorsException(validator.GetInvalidMessages<Customer>(customer));
        }

        Customer MaterializeCustomerFromDto(CustomerDTO customerDTO)
        {
            //create the current instance with changes from customerDTO
            var address = new Address(customerDTO.AddressCity, customerDTO.AddressZipCode, customerDTO.AddressAddressLine1, customerDTO.AddressAddressLine2);

            Country country = new Country("Spain", "es-ES");
            country.ChangeCurrentIdentity(customerDTO.CountryId);

            var current = CustomerFactory.CreateCustomer(customerDTO.FirstName,
                                                         customerDTO.LastName,
                                                         customerDTO.Telephone,
                                                         customerDTO.Company,
                                                         country,
                                                         address);

            current.SetTheCountryReference(customerDTO.Id);

            //set credit
            current.ChangeTheCurrentCredit(customerDTO.CreditLimit);

            //set picture
            var picture = new Picture { RawPhoto = customerDTO.PictureRawPhoto };
            picture.ChangeCurrentIdentity(current.Id);

            current.ChangePicture(picture);

            //set identity
            current.ChangeCurrentIdentity(customerDTO.Id);


            return current;
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            //dispose all resources
            _countryRepository.Dispose();
            _customerRepository.Dispose();
        }

        #endregion
    }
}
