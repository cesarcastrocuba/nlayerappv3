namespace NLayerApp.Application.BlogBoundedContext.DTO.Profiles
{
    using AutoMapper;
    using NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.ImageAgg;
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<Image, ImageDTO>()
                .ForMember(dto => dto.ImageId, m => m.MapFrom(e => e.ImageId))
                .ForMember(dto => dto.Title, m => m.MapFrom(e => e.Title))
                .ForMember(dto => dto.Url, m => m.MapFrom(e => e.Url))
                .PreserveReferences()
                .ReverseMap();
        }
    }
}
