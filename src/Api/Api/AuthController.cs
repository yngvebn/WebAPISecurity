using System.Web.Http;
using Api.Infrastructure.Security;

namespace Api.Api
{
    public class AuthController : ApiController
    {
        [AuthorizationTokenFilter]
        public string Get()
        {
            return "Done.";
        }
    }
}