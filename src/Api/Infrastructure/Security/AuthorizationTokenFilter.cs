using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Api.Infrastructure.RSA;
using RsaHelpers;

namespace Api.Infrastructure.Security
{
    public class AuthorizationTokenFilterAttribute : ActionFilterAttribute
    {
        private static string _privateKey =
           "<RSAKeyValue><Modulus>s6lpjspk+3o2GOK5TM7JySARhhxE5gB96e9XLSSRuWY2W9F951MfistKRzVtg0cjJTdSk5mnWAVHLfKOEqp8PszpJx9z4IaRCwQ937KJmn2/2VyjcUsCsor+fdbIHOiJpaxBlsuI9N++4MgF/jb0tOVudiUutDqqDut7rhrB/oc=</Modulus><Exponent>AQAB</Exponent><P>3J2+VWMVWcuLjjnLULe5TmSN7ts0n/TPJqe+bg9avuewu1rDsz+OBfP66/+rpYMs5+JolDceZSiOT+ACW2Neuw==</P><Q>0HogL5BnWjj9BlfpILQt8ajJnBHYrCiPaJ4npghdD5n/JYV8BNOiOP1T7u1xmvtr2U4mMObE17rZjNOTa1rQpQ==</Q><DP>jbXh2dVQlKJznUMwf0PUiy96IDC8R/cnzQu4/ddtEe2fj2lJBe3QG7DRwCA1sJZnFPhQ9svFAXOgnlwlB3D4Gw==</DP><DQ>evrP6b8BeNONTySkvUoMoDW1WH+elVAH6OsC8IqWexGY1YV8t0wwsfWegZ9IGOifojzbgpVfIPN0SgK1P+r+kQ==</DQ><InverseQ>LeEoFGI+IOY/J+9SjCPKAKduP280epOTeSKxs115gW1b9CP4glavkUcfQTzkTPe2t21kl1OrnvXEe5Wrzkk8rA==</InverseQ><D>HD0rn0sGtlROPnkcgQsbwmYs+vRki/ZV1DhPboQJ96cuMh5qeLqjAZDUev7V2MWMq6PXceW73OTvfDRcymhLoNvobE4Ekiwc87+TwzS3811mOmt5DJya9SliqU/ro+iEicjO4v3nC+HujdpDh9CVXfUAWebKnd7Vo5p6LwC9nIk=</D></RSAKeyValue>";

        private static string _publicKey =
            "<RSAKeyValue><Modulus>s6lpjspk+3o2GOK5TM7JySARhhxE5gB96e9XLSSRuWY2W9F951MfistKRzVtg0cjJTdSk5mnWAVHLfKOEqp8PszpJx9z4IaRCwQ937KJmn2/2VyjcUsCsor+fdbIHOiJpaxBlsuI9N++4MgF/jb0tOVudiUutDqqDut7rhrB/oc=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        private RsaEncrypter _rsaEncrypter;
        public AuthorizationTokenFilterAttribute()
        {
            _rsaEncrypter = new RsaEncrypter(_privateKey, _publicKey);
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string token;
            token = actionContext.Request.Headers.GetValues("Authorization-Token").FirstOrDefault();
            if (token == null)
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Missing Authorization-Token")
                };
                return;
            }
            string userName = _rsaEncrypter.Decrypt(token);

            if (userName != "yngvebn")
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                    {
                        Content = new StringContent("Unauthorized User")
                    };
            }
        }

    }
}