using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using NLayerApp.Infrastructure.Crosscutting.Localization;
using System.Diagnostics;
using System.Linq;

namespace NLayerApp.DistributedServices.Seedwork.Filters
{
    public class LoggerAttribute : ActionFilterAttribute
    {
        private Stopwatch watch;
        private readonly ILogger<LoggerAttribute> logger;

        private ILocalization resources;
        public LoggerAttribute(ILogger<LoggerAttribute> _logger)
        {
            logger = _logger;
            resources = LocalizationFactory.CreateLocalResources();
        }
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            watch = new Stopwatch();
            watch.Start();

            var parameter = actionContext.ActionArguments.FirstOrDefault();
            string logMessage = string.Format(resources.GetStringResource(LocalizationKeys.Distributed_Services.info_OnExecuting), actionContext.ActionDescriptor.DisplayName, watch.ElapsedMilliseconds);
            object parameterValue = parameter.Value;
            string values = parameterValue != null ? Newtonsoft.Json.JsonConvert.SerializeObject(parameterValue) : null;
            logMessage += string.Format(resources.GetStringResource(LocalizationKeys.Distributed_Services.info_Parameter), values ?? "/null/");
            logger.LogInformation(logMessage);
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
            string logMessage = string.Format(resources.GetStringResource(LocalizationKeys.Distributed_Services.info_OnExecuted), actionExecutedContext.ActionDescriptor.DisplayName , watch.ElapsedMilliseconds);
            logger.LogInformation(logMessage);
            watch.Stop();
        }
    }
}
