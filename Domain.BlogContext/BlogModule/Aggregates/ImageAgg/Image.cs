namespace NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.ImageAgg
{
    using NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.PostAgg;
    using NLayerApp.Domain.Seedwork;
    using NLayerApp.Infrastructure.Crosscutting.Localization;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    /// <summary>
    /// An image can be part of a post and contains the URL of an image resource in the web.
    /// </summary>
    public class Image : Entity, IValidatableObject
    {
        public int ImageId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        [JsonIgnore]
        public int PostId { get; set; }
        [JsonIgnore]
        public Post Post { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var resources = LocalizationFactory.CreateLocalResources();
            var validationResults = new List<ValidationResult>();

            if (string.IsNullOrEmpty(Title))
                validationResults.Add(new ValidationResult(string.Format(resources.GetStringResource(LocalizationKeys.Domain.validation_PropertyIsEmptyOrNull), "Title", "Image"), new string[] { "Title" }));

            if (string.IsNullOrEmpty(Url))
                validationResults.Add(new ValidationResult(string.Format(resources.GetStringResource(LocalizationKeys.Domain.validation_PropertyIsEmptyOrNull), "Url", "Image"), new string[] { "Content" }));

            if (PostId < 1)
                validationResults.Add(new ValidationResult(string.Format(resources.GetStringResource(LocalizationKeys.Domain.validation_PropertyOutOfRange), "PostId", "Image", "0", "Int32.MaxValue"), new string[] { "PostId" }));

            return validationResults;
        }
    }
}
