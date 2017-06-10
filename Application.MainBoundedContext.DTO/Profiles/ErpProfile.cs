namespace NLayerApp.Application.MainBoundedContext.DTO.Profiles
{
    using AutoMapper;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
    public class ErpProfile
        : Profile
    {
        public ErpProfile()
        {
            //book => book dto
            CreateMap<Book, BookDTO>();

            //country => countrydto
            CreateMap<Country, CountryDTO>();

            //customer => customerlistdto
            CreateMap<Customer, CustomerListDTO>();

            //customer => customerdto
            var customerMappingExpression = CreateMap<Customer, CustomerDTO>();

            //order => orderlistdto
            var orderListMappingExpression = CreateMap<Order, OrderListDTO>();
            orderListMappingExpression.ForMember(dto => dto.TotalOrder, mc => mc.MapFrom(e => e.GetOrderTotal()));
            orderListMappingExpression.ForMember(dto => dto.ShippingAddress, mc => mc.MapFrom(e => e.ShippingInformation.ShippingAddress));
            orderListMappingExpression.ForMember(dto => dto.ShippingCity, mc => mc.MapFrom(e => e.ShippingInformation.ShippingCity));
            orderListMappingExpression.ForMember(dto => dto.ShippingName, mc => mc.MapFrom(e => e.ShippingInformation.ShippingName));
            orderListMappingExpression.ForMember(dto => dto.ShippingZipCode, mc => mc.MapFrom(e => e.ShippingInformation.ShippingZipCode));
            
            //order => orderdto
            var orderMappingExpression = CreateMap<Order, OrderDTO>();

            orderMappingExpression.ForMember(dto => dto.ShippingAddress, (map) => map.MapFrom(o => o.ShippingInformation.ShippingAddress));
            orderMappingExpression.ForMember(dto => dto.ShippingCity, (map) => map.MapFrom(o => o.ShippingInformation.ShippingCity));
            orderMappingExpression.ForMember(dto => dto.ShippingName, (map) => map.MapFrom(o => o.ShippingInformation.ShippingName));
            orderMappingExpression.ForMember(dto => dto.ShippingZipCode, (map) => map.MapFrom(o => o.ShippingInformation.ShippingZipCode));

            //orderline => orderlinedto
            var lineMapperExpression = CreateMap<OrderLine, OrderLineDTO>();
            lineMapperExpression.ForMember(dto => dto.Discount, mc => mc.MapFrom(o => o.Discount * 100));

            //product => productdto
            CreateMap<Product, ProductDTO>();

            //software => softwaredto
            CreateMap<Software, SoftwareDTO>();

        }
    }
}
