using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Security;
using RsaHelpers;

namespace Api.Infrastructure.Security
{
    public class AuthorizationTokenFilter : AuthorizationFilterAttribute
    {
        private readonly IRsa _rsa;
        public AuthorizationTokenFilter(IRsa rsa)
        {
            _rsa = rsa;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {

            IEnumerable<string> values = new List<string>();
            actionContext.Request.Headers.TryGetValues("Session-Token", out values);
            if (values == null) return;
            
            // do something with the session-token

            base.OnAuthorization(actionContext);
        }
        
        
    }

}