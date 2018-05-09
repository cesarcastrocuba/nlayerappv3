

namespace NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.PostAgg
{
    using NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.BlogAgg;
    using NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.ImageAgg;
    using NLayerApp.Domain.Seedwork;
    using NLayerApp.Infrastructure.Crosscutting.Localization;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    /// <summary>
    /// A post is an inidivual content item. It always belongs to a blog and can contain several images.
    /// </summary>
    public class Post : Entity, IValidatableObject
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        [JsonIgnore]
        public int BlogId { get; set; }
        [JsonIgnore]
        public virtual Blog Blog { get; set; }
        public virtual List<Image> Images{ get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var resources = LocalizationFactory.CreateLocalResources();
            var validationResults = new List<ValidationResult>();
            
            if (string.IsNullOrEmpty(Title))
                validationResults.Add(new ValidationResult(string.Format(resources.GetStringResource(LocalizationKeys.Domain.validation_PropertyIsEmptyOrNull), "Title", "Post"), new string[] { "Title" }));

            if (string.IsNullOrEmpty(Content))
                validationResults.Add(new ValidationResult(string.Format(resources.GetStringResource(LocalizationKeys.Domain.validation_PropertyIsEmptyOrNull), "Content", "Post"), new string[] { "Content" }));

            if (BlogId < 1)
                validationResults.Add(new ValidationResult(string.Format(resources.GetStringResource(LocalizationKeys.Domain.validation_PropertyOutOfRange), "BlogId", "Blog", "0", "Int32.MaxValue"), new string[] { "BlogId" }));

            return validationResults;
        }
    }
}
