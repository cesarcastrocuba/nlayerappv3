namespace NLayerApp.Application.BlogBoundedContext.DTO.Profiles
{
    using AutoMapper;
    using NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.BlogAgg;
    public class BlogProfile : Profile
    {
        public BlogProfile()
        {
            CreateMap<Blog, BlogDTO>()
                .ForMember(dto => dto.BlogId , m => m.MapFrom(e => e.BlogId))
                .ForMember(dto => dto.Name, m => m.MapFrom(e => e.Name))
                .ForMember(dto => dto.Url, m => m.MapFrom(e => e.Url))
                .ForMember(dto => dto.Rating, m => m.MapFrom(e => e.Rating))
                .ForMember(dto => dto.Posts, m => m.MapFrom(e=> e.Posts))
                .ForMember(dto => dto.CreatedAt, m => m.MapFrom(e => e.CreatedAt))
                .ForMember(dto => dto.CreatedBy, m => m.MapFrom(e => e.CreatedBy))
                .ForMember(dto => dto.LastModifiedAt, m => m.MapFrom(e => e.LastModifiedAt))
                .ForMember(dto => dto.LastModifiedBy, m => m.MapFrom(e => e.LastModifiedBy))

                .PreserveReferences()
                .ReverseMap();
        }
    }
}
