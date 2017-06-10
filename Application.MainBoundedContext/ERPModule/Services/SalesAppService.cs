namespace NLayerApp.Application.MainBoundedContext.ERPModule.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NLayerApp.Application.MainBoundedContext.DTO;
    using NLayerApp.Application.Seedwork;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
    using NLayerApp.Infrastructure.Crosscutting.Validator;
    using NLayerApp.Infrastructure.Crosscutting.Localization;
    using Microsoft.Extensions.Logging;

    public class SalesAppService
        : ISalesAppService
    {
        #region Members

        readonly IOrderRepository _orderRepository;
        readonly IProductRepository _productRepository;
        readonly ICustomerRepository _customerRepository;

        readonly ILogger _logger;
        readonly ILocalization _resources;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance of sales management service
        /// </summary>
        /// <param name="orderRepository">The associated order repository</param>
        /// <param name="productRepository">The associated product repository</param>
        /// <param name="customerRepository">The associated customer repository</param>
        public SalesAppService(IProductRepository productRepository,//associated product repository
                               IOrderRepository orderRepository,//associated order repository
                               ICustomerRepository customerRepository,//the associated customer repository
                               ILogger<SalesAppService> logger) 
        {
            if (orderRepository == null)
                throw new ArgumentNullException("orderRepository");

            if (productRepository == null)
                throw new ArgumentNullException("productRepository");

            if (customerRepository == null)
                throw new ArgumentNullException("customerRepository");

            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;

            _logger = logger;
            _resources = LocalizationFactory.CreateLocalResources();
        }
        #endregion

        #region ISalesAppService Members

        public List<OrderListDTO> FindOrders(int pageIndex, int pageCount)
        {
            if (pageIndex < 0 || pageCount <= 0)
                throw new ArgumentException(_resources.GetStringResource(LocalizationKeys.Application.warning_InvalidArgumentForFindOrders));

            //recover orders in paged fashion
            var orders = _orderRepository.GetPaged<DateTime>(pageIndex, pageCount, o => o.OrderDate, false);

            if (orders != null
                &&
                orders.Any())
            {
                return orders.ProjectedAsCollection<OrderListDTO>();
            }
            else // no data
                return null;
        }
        public List<OrderListDTO> FindOrders(DateTime? dateFrom, DateTime? dateTo)
        {
            //create the specification ( how to filter orders from dates..)
            var spec = OrdersSpecifications.OrderFromDateRange(dateFrom, dateTo);

            //recover orders
            var orders = _orderRepository.AllMatching(spec);

            if (orders != null
               &&
               orders.Any())
            {
                return orders.ProjectedAsCollection<OrderListDTO>();
            }
            else //no data
                return null;

        }
        
        public List<OrderListDTO> FindOrders(Guid customerId)
        {
            var orders = _orderRepository.GetFiltered(o => o.CustomerId == customerId);

            if (orders != null
               &&
               orders.Any())
            {
                return orders.ProjectedAsCollection<OrderListDTO>();
            }
            else //no data..
                return null;

        }
        
        public List<ProductDTO> FindProducts(int pageIndex, int pageCount)
        {
            if (pageIndex < 0 || pageCount <= 0)
                throw new ArgumentException(_resources.GetStringResource(LocalizationKeys.Application.warning_InvalidArgumentForFindProducts));

            //recover products
            var products = _productRepository.GetPaged<string>(pageIndex, pageCount, p => p.Title, false);

            if (products != null
                &&
                products.Any())
            {
                return products.ProjectedAsCollection<ProductDTO>();
            }
            else // no data
                return null;
        }

        public List<ProductDTO> FindProducts(string text)
        {
            //create the specification ( howto find products for any string ) 
            var spec = ProductSpecifications.ProductFullText(text);

            //recover products
            var products = _productRepository.AllMatching(spec);

            //adapt results
            return products.ProjectedAsCollection<ProductDTO>();
        }

        public OrderDTO AddNewOrder(OrderDTO orderDto)
        {
            //if orderdto data is not valid
            if (orderDto == null || orderDto.CustomerId == Guid.Empty)
                throw new ArgumentException(_resources.GetStringResource(LocalizationKeys.Application.warning_CannotAddOrderWithNullInformation));
            
            var customer = _customerRepository.Get(orderDto.CustomerId);

            if (customer != null)
            {
                //Create a new order entity
                var newOrder = CreateNewOrder(orderDto, customer);

                if (newOrder.IsCreditValidForOrder()) //if total order is less than credit 
                {
                    //save order
                    SaveOrder(newOrder);

                    return newOrder.ProjectedAs<OrderDTO>();
                }
                else //total order is greater than credit
                {
                    _logger.LogInformation(_resources.GetStringResource(LocalizationKeys.Application.info_OrderTotalIsGreaterCustomerCredit));
                    return null;
                }
            }
            else
            {
                _logger.LogWarning(_resources.GetStringResource(LocalizationKeys.Application.warning_CannotCreateOrderForNonExistingCustomer));
                return null;
            }
        }
        public SoftwareDTO AddNewSoftware(SoftwareDTO softwareDTO)
        {
            if (softwareDTO == null)
                throw new ArgumentException(_resources.GetStringResource(LocalizationKeys.Application.warning_CannotAddSoftwareWithNullInformation));

            //Create the softare entity
            var newSoftware = new Software(softwareDTO.Title, softwareDTO.Description,softwareDTO.LicenseCode);

            //set unit price and stock
            newSoftware.ChangeUnitPrice(softwareDTO.UnitPrice);
            newSoftware.IncrementStock(softwareDTO.AmountInStock);

            //Assign the poid
            newSoftware.GenerateNewIdentity();

            //save software
            SaveProduct(newSoftware);

            //return software dto
            return newSoftware.ProjectedAs<SoftwareDTO>();
        }
        public BookDTO AddNewBook(BookDTO bookDTO)
        {
            if (bookDTO == null)
                throw new ArgumentNullException(_resources.GetStringResource(LocalizationKeys.Application.warning_CannotAddSoftwareWithNullInformation));

            //Create the book entity
            var newBook = new Book(bookDTO.Title, bookDTO.Description,bookDTO.Publisher,bookDTO.ISBN);
            
            //set stock and unit price
            newBook.IncrementStock(bookDTO.AmountInStock);
            newBook.ChangeUnitPrice(bookDTO.UnitPrice);

            //Assign the poid
            newBook.GenerateNewIdentity();

            //save software
            SaveProduct(newBook);

            //return software dto
            return newBook.ProjectedAs<BookDTO>();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            //dispose all resources
            _orderRepository.Dispose();
            _productRepository.Dispose();
            _customerRepository.Dispose();
        }

        #endregion

        #region Private Methods

        void SaveOrder(Order order)
        {
            var entityValidator = EntityValidatorFactory.CreateValidator();

            if (entityValidator.IsValid(order))//if entity is valid save. 
            {
                //add order and commit changes
                _orderRepository.Add(order);
                _orderRepository.UnitOfWork.Commit();
            }
            else // if not valid throw validation errors
                throw new ApplicationValidationErrorsException(entityValidator.GetInvalidMessages(order));
        }
        Order CreateNewOrder(OrderDTO dto, Customer associatedCustomer)
        {
            //Create a new order entity from factory
            Order newOrder = OrderFactory.CreateOrder(associatedCustomer,
                                                     dto.ShippingName,
                                                     dto.ShippingCity,
                                                     dto.ShippingAddress,
                                                     dto.ShippingZipCode);

            //if have lines..add
            if (dto.OrderLines != null)
            {
                foreach (var line in dto.OrderLines) //add order lines
                    newOrder.AddNewOrderLine(line.ProductId, line.Amount, line.UnitPrice, line.Discount / 100);
            }

            return newOrder;
        }
        void SaveProduct(Product product)
        {
            var entityValidator = EntityValidatorFactory.CreateValidator();

            if (entityValidator.IsValid(product)) // if is valid
            {
                _productRepository.Add(product);
                _productRepository.UnitOfWork.Commit();
            }
            else //if not valid, throw validation errors
                throw new ApplicationValidationErrorsException(entityValidator.GetInvalidMessages(product));
        }

        #endregion
    }
}
