namespace NLayerApp.Application.BlogBoundedContext.DTO
{
    using FluentValidation.Attributes;
    using NLayerApp.Application.BlogBoundedContext.DTO.Validations;
    using NLayerApp.Domain.Seedwork;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    [Validator(typeof(PostDTOValidator))]
    public class PostDTO : Entity
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        [JsonIgnore]
        public int BlogId { get; set; }
        [JsonIgnore]
        public BlogDTO Blog { get; set; }

        public List<ImageDTO> Images { get; set; }
    }
}
