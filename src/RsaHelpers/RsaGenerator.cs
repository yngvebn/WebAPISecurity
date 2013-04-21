using System.Security.Cryptography;

namespace RsaHelpers
{
    public class RsaGenerator
    {
        public static IRsaKeys GenerateKeys()
        {
            var rsa = new RSACryptoServiceProvider();
            return new RsaKeyPair
                {
                    PublicKey = rsa.ToXmlString(false),
                    PrivateKey = rsa.ToXmlString(true)
                };
        }

        public class RsaKeyPair: IRsaKeys
        {
            public string PublicKey { get; internal set; }
            public string PrivateKey { get; internal set; }
        }
    }

}