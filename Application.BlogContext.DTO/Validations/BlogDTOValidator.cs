namespace NLayerApp.Application.BlogBoundedContext.DTO.Validations
{
    using FluentValidation;
    using NLayerApp.Infrastructure.Crosscutting.Localization;
    public class BlogDTOValidator : AbstractValidator<BlogDTO>
    {
        public BlogDTOValidator()
        {
            var _resources = LocalizationFactory.CreateLocalResources();

            RuleFor(b => b.Name)
                .NotEmpty().WithMessage(_resources.GetStringResource(LocalizationKeys.Application.validation_BlogDto_Empty_Name));

            RuleFor(b => b.Url)
                .NotEmpty().WithMessage(_resources.GetStringResource(LocalizationKeys.Application.validation_BlogDto_Empty_Url));

            RuleFor(b => b.Rating)
                .GreaterThan(-1).LessThan(6).WithMessage(_resources.GetStringResource(LocalizationKeys.Application.validation_BlogDto_Invalid_Rating));
        }
    }
}
