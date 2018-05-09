namespace NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.BlogAgg
{
    using NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.PostAgg;
    using NLayerApp.Domain.Seedwork;
    using NLayerApp.Infrastructure.Crosscutting.Localization;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    

    /// <summary>
    /// A blog is the main instance in this API. I contains a list of posts.
    /// </summary>
    public class Blog : AuditableEntity, IValidatableObject
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Rating { get; set; }
        public virtual List<Post> Posts { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var resources = LocalizationFactory.CreateLocalResources();
            var validationResults = new List<ValidationResult>();

            if (string.IsNullOrEmpty(Name))
                validationResults.Add(new ValidationResult(string.Format(resources.GetStringResource(LocalizationKeys.Domain.validation_PropertyIsEmptyOrNull), "Name", "Blog"), new string[] { "Name" }));

            if (string.IsNullOrEmpty(Url))
                validationResults.Add(new ValidationResult(string.Format(resources.GetStringResource(LocalizationKeys.Domain.validation_PropertyIsEmptyOrNull), "Url", "Blog"), new string[] { "Url" }));
            
            if (Rating < 0 && Rating > 5)
                validationResults.Add(new ValidationResult(string.Format(resources.GetStringResource(LocalizationKeys.Domain.validation_PropertyOutOfRange), "Rating", "Blog", "-1", "5"), new string[] { "Rating" }));

            return validationResults;
        }
    }
}
