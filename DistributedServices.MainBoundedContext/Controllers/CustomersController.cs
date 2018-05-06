using Microsoft.AspNetCore.Mvc;
using NLayerApp.Application.MainBoundedContext.DTO;
using NLayerApp.Application.MainBoundedContext.ERPModule.Services;
using NLayerApp.DistributedServices.Seedwork.Filters;
using System;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NLayerApp.DistributedServices.MainBoundedContext.Controllers
{
    [Route("api/[controller]")]
    [ServiceFilter(typeof(CustomExceptionFilterAttribute))]
    public class CustomersController : Controller, IDisposable
    {
        private readonly ICustomerAppService _customerAppService;
        public CustomersController(ICustomerAppService customerAppService)
        {
            _customerAppService = customerAppService;
        }
        // GET: api/customers?filter=filter
        [HttpGet]
        public IEnumerable<CustomerListDTO> Get(string filter)
        {
            return _customerAppService.FindCustomers(filter);
        }
        // GET: api/customers/getpaged/?pageIndex=1&pageCount=1
        [HttpGet("GetPaged")]
        public IEnumerable<CustomerListDTO> Get(int pageIndex, int pageCount)
        {
            return _customerAppService.FindCustomers(pageIndex, pageCount);
        }
        // GET: api/customers/1
        [HttpGet("{id}")]
        public CustomerDTO Get(Guid id)
        {
            return _customerAppService.FindCustomer(id);
        }
        // POST: api/customers/
        [HttpPost]
        public CustomerDTO Post([FromBody]CustomerDTO customer)
        {
            return _customerAppService.AddNewCustomer(customer);
        }
        // PUT: api/customers/
        [HttpPut("{id}")]
        public void Put([FromBody]CustomerDTO customer)
        {
            _customerAppService.UpdateCustomer(customer);
        }
        // DELETE: api/customers/
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            _customerAppService.RemoveCustomer(id);
        }

        //// GET: api/customers/countries
        [HttpGet("GetCountries")]
        public IEnumerable<CountryDTO> Countries(string filter)
        {
            return _customerAppService.FindCountries(filter);
        }
        //// GET: api/customers/getpagedcountries?pageIndex=1&pageCount=1
        [HttpGet("GetPagedCountries")]
        public IEnumerable<CountryDTO> Countries(int pageIndex, int pageCount)
        {
            return _customerAppService.FindCountries(pageIndex, pageCount);
        }

        #region IDisposable Members
        public void Dispose()
        {
            //dispose all resources
            _customerAppService.Dispose();
        }
        #endregion
    }
}
