using System.ComponentModel.DataAnnotations;

namespace NLayerApp.Infrastructure.Crosscutting.Tests.Classes
{
    class EntityWithValidationAttribute
    {
        [Required(ErrorMessage = "This is a required property")]
        public string RequiredProperty { get; set; }
    }
}
