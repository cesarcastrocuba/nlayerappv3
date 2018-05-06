using Microsoft.AspNetCore.Mvc;
using NLayerApp.Application.MainBoundedContext.DTO;
using NLayerApp.Application.MainBoundedContext.ERPModule.Services;
using System;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NLayerApp.DistributedServices.MainBoundedContext.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller, IDisposable
    {
        private readonly ISalesAppService _salesAppService;
        public OrdersController(ISalesAppService salesAppService)
        {
            _salesAppService = salesAppService;
        }

        // GET: api/orders/?pageIndex=1&pageCount=1
        [HttpGet]
        public IEnumerable<OrderListDTO> Get(int pageIndex, int pageCount)
        {
            return _salesAppService.FindOrders(pageIndex, pageCount);
        }

        // GET: api/orders/?from=20170101&to=20170131
        [HttpGet("GetBetweenDates")]
        public IEnumerable<OrderListDTO> Get(DateTime from, DateTime to)
        {
            return _salesAppService.FindOrders(from, to);
        }

        // GET api/orders/customers/1
        [HttpGet("[action]/{id}")]
        public IEnumerable<OrderListDTO> Customers(Guid customerId)
        {
            return _salesAppService.FindOrders(customerId);
        }

        // POST api/orders
        [HttpPost]
        public OrderDTO Post([FromBody]OrderDTO order)
        {
            return _salesAppService.AddNewOrder(order);
        }

        // POST api/orders/software
        [HttpPost("Software")]
        public SoftwareDTO Software([FromBody]SoftwareDTO software)
        {
            return _salesAppService.AddNewSoftware(software);
        }

        // POST api/orders/book
        [HttpPost("Book")]
        public BookDTO Book([FromBody]BookDTO book)
        {
            return _salesAppService.AddNewBook(book);
        }

        // GET: api/orders/getpagedproducts/?pageIndex=1&pageCount=1
        [HttpGet("GetPagedProducts")]
        public IEnumerable<ProductDTO> Products(int pageIndex, int pageCount)
        {
            return _salesAppService.FindProducts(pageIndex, pageCount);
        }

        // GET: api/orders/getproducts?filter=filter
        [HttpGet("GetProducts")]
        public IEnumerable<ProductDTO> Products(string filter)
        {
            return _salesAppService.FindProducts(filter);
        }

        #region IDisposable Members
        public void Dispose()
        {
            //dispose all resources
            _salesAppService.Dispose();
        }
        #endregion
    }
}
