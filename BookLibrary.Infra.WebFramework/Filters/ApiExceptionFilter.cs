using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookLibrary.Infra.WebFramework.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IModelMetadataProvider _modelMetadataProvider;
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(IWebHostEnvironment hostingEnvironment,
        IModelMetadataProvider modelMetadataProvider,
        ILogger<ApiExceptionFilter> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _modelMetadataProvider = modelMetadataProvider;
            this._logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (!_hostingEnvironment.IsDevelopment())
            {
                return;
            }
            _logger.LogError(context.Exception.Message, context);
        }
    }
}
