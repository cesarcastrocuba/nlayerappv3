namespace NLayerApp.Application.BlogBoundedContext.DTO.Validations
{
    using FluentValidation;
    using NLayerApp.Infrastructure.Crosscutting.Localization;
    public class PostDTOValidator : AbstractValidator<PostDTO>
    {
        public PostDTOValidator()
        {
            var _resources = LocalizationFactory.CreateLocalResources();

            RuleFor(b => b.Title)
                .NotEmpty().WithMessage(_resources.GetStringResource(LocalizationKeys.Application.validation_PostDto_Empty_Title));

            RuleFor(b => b.Content)
                .NotEmpty().WithMessage(_resources.GetStringResource(LocalizationKeys.Application.validation_PostDto_Empty_Content));

            RuleFor(b => b.BlogId)
                .LessThan(0).WithMessage(_resources.GetStringResource(LocalizationKeys.Application.validation_PostDto_Invalid_BlogId));
        }
    }
}
