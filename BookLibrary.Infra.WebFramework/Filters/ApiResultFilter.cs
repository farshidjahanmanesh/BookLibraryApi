using BookLibrary.Infra.WebFramework.Api;
using BookLibrary.Infra.WebFramework.Api.ApiResponses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary.Infra.WebFramework.Filters
{
    public class ApiResultFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var url = $"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}{context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}";
            switch (context.Result)
            {
                case OkObjectResult okObj:
                    var apiOkObjResult = new ApiOkResponse(okObj.Value, url);
                    context.Result = new JsonResult(apiOkObjResult) { StatusCode = apiOkObjResult.StatusCode };
                    break;
                case OkResult:
                    var apiOkResult = new ApiOkResponse(null, url);
                    context.Result = new JsonResult(apiOkResult) { StatusCode = apiOkResult.StatusCode };
                    break;
                case ContentResult contentResult:
                    var apicontentResult = new ApiOkResponse(contentResult.Content, url);
                    context.Result = new JsonResult(apicontentResult) { StatusCode = apicontentResult.StatusCode };
                    break;

                case ObjectResult objResult:

                    switch (objResult.StatusCode)
                    {
                        case 400:
                            var apiBadRequestResult = new ApiBadRequestResponse(url, GetInformationDatas(objResult));
                            context.Result = new JsonResult(apiBadRequestResult) { StatusCode = apiBadRequestResult.StatusCode };
                            break;
                        case 404:
                            var apiNotFoundResult = new ApiNotFoundResponse(url, GetInformationDatas(objResult));
                            context.Result = new JsonResult(apiNotFoundResult) { StatusCode = apiNotFoundResult.StatusCode };
                            break;
                        case 415:
                            var apiUnSupportResult = new ApiUnSupportResponse(url);
                            context.Result = new JsonResult(apiUnSupportResult) { StatusCode = apiUnSupportResult.StatusCode };
                            break;
                        case null:
                            if (objResult.Value is not ApiResponse)
                            {
                                var apiObjResult = new ApiOkResponse(objResult.Value, url);
                                context.Result = new JsonResult(apiObjResult) { StatusCode = apiObjResult.StatusCode };
                            }
                            break;
                    }
                    break;
            }
            base.OnResultExecuting(context);
        }

        private IEnumerable<string> GetInformationDatas(ObjectResult obj)
        {
            List<string> result = new();
            switch (obj.Value)
            {
                case ValidationProblemDetails validationProblemDetails:
                    var errorMessages = validationProblemDetails.Errors.SelectMany(p => p.Value).Distinct();
                    result.AddRange(errorMessages);
                    break;
                case SerializableError errors:
                    var errorMessages2 = errors.SelectMany(p => (string[])p.Value).Distinct();
                    result.AddRange(errorMessages2);
                    break;
                case var value when value != null && !(value is ProblemDetails):
                    result.Add(obj.Value.ToString());
                    break;
            }
            return result;
        }
    }
}
