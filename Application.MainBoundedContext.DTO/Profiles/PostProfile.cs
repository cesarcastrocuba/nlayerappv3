namespace NLayerApp.Application.MainBoundedContext.DTO.Profiles
{
    using AutoMapper;
    using NLayerApp.Domain.MainBoundedContext.Aggregates.PostAgg;
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, PostDTO>()
                .ForMember(dto => dto.PostId, m => m.MapFrom(e => e.PostId))
                .ForMember(dto => dto.Title, m => m.MapFrom(e => e.Title))
                .ForMember(dto => dto.Content, m => m.MapFrom(e => e.Content))
                .ForMember(dto => dto.Images, m => m.MapFrom(e =>e.Images))
                .PreserveReferences()
                .ReverseMap();
        }
    }
}
