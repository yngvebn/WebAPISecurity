using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Api.Infrastructure.Security
{
    public class HttpsActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!String.Equals(actionContext.Request.RequestUri.Scheme, "https", StringComparison.OrdinalIgnoreCase))
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("HTTPS Required")
                };
            }
        }
    }

}