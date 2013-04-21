using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using System.Web.Security;
using Api.Infrastructure.Security;
using RsaHelpers;

namespace Api.Api
{
    public class AuthController : ApiController
    {

        private readonly IRsa _rsa;
        private readonly ISessionRepository _sessionRepository;

        public AuthController(IRsa rsa, ISessionRepository sessionRepository)
        {
            _rsa = rsa;
            _sessionRepository = sessionRepository;
        }

        public SessionToken Get()
        {
            var result = new HttpResponseMessage();

            IEnumerable<string> values = new List<string>();
            Request.Headers.TryGetValues("Authorization-Token", out values);
            if (values == null) return null;
            var token = values.FirstOrDefault();
            if (token != null)
            {
                string authInfo = _rsa.Decrypt(token);
                var usernameSplitIndex = authInfo.IndexOf('=');
                var username = authInfo.Substring(0, usernameSplitIndex);
                var password = authInfo.Substring(usernameSplitIndex + 1);

            }

            return null;
        }
    }

    public interface ISessionRepository
    {

    }

    public class SessionToken
    {
        public DateTime ValidUntil { get; set; }
        public string Token { get; set; }
    }
}