namespace NLayerApp.Application.BlogBoundedContext.DTO
{
    using FluentValidation.Attributes;
    using NLayerApp.Application.BlogBoundedContext.DTO.Validations;
    using NLayerApp.Domain.Seedwork;
    using System.Collections.Generic;

    [Validator(typeof(BlogDTOValidator))]
    public class BlogDTO : AuditableEntity
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Rating { get; set; }
        public List<PostDTO> Posts { get; set; }
    }
}
