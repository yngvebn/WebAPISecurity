using System.Security.Cryptography;

namespace Api.Infrastructure.RSA
{
    public class RsaGenerator
    {
        public static string GenerateKey(bool includePrivateParameter)
        {
            var rsa = new RSACryptoServiceProvider();
            var privateParameters = rsa.ExportParameters(includePrivateParameter);
            return rsa.ToXmlString(includePrivateParameter);
        }
    }

}