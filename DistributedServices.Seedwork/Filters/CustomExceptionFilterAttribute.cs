using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using NLayerApp.Application.Seedwork;
using System;
using System.Net;

namespace NLayerApp.DistributedServices.Seedwork.Filters
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IModelMetadataProvider _modelMetadataProvider;

        private readonly ILogger _logger;

        public CustomExceptionFilterAttribute(
            IHostingEnvironment hostingEnvironment,
            IModelMetadataProvider modelMetadataProvider,
            ILoggerFactory loggerFactory)
        {
            _hostingEnvironment = hostingEnvironment;
            _modelMetadataProvider = modelMetadataProvider;

            _logger = loggerFactory.CreateLogger<CustomExceptionFilterAttribute>();
        }

        public override void OnException(ExceptionContext context)
        {
            //if (!_hostingEnvironment.IsDevelopment())
            //{
            //    // do nothing
            //    return;
            //}

           var exception = context.Exception;
            var message = exception.Message;

            if (exception is ApplicationValidationErrorsException)
            {
                var validationErrors = ((ApplicationValidationErrorsException)exception).ValidationErrors;

                if (validationErrors != null)
                {
                    foreach (var error in validationErrors)
                    {
                        message += Environment.NewLine + error;
                    }
                }
            }

            var controller = context.ActionDescriptor.RouteValues["controller"];
            var action = context.ActionDescriptor.RouteValues["action"];
            var result = new JsonResult(new { controller, action, message });

            _logger.LogError(null, exception, result.Value.ToString());

            result.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Result = new BadRequestObjectResult(result);

        }
    }
}
