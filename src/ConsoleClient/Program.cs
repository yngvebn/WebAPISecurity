using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RsaHelpers;

namespace ConsoleClient
{
    class Program
    {

        private static Rsa _rsa;
        class RsaKeys: IRsaKeys
        {
            public string PublicKey { get; private set; }
            public string PrivateKey { get; private set; }

            public RsaKeys()
            {
                
                PublicKey =
           "<RSAKeyValue><Modulus>1TGTGmcJRlLCRXo9M38P9LDWIkBXEbKHefxQru13votIB0ODnlKPK2GOpEfJcr66pnjRdON/xcvEojKLjOFC1l1lCzrwTnCD8//aY8YmTgoxT6K4ytJrlUuEusnMzPJuZviYUSl2Takbe0vKilaztJi0nj8HlJFDC8Vu/JF/S0k=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
         
            }
        }
        static void Main(string[] args)
        {
             _rsa = new Rsa(new RsaKeys());
            var token = _rsa.Encrypt("yngvebn="+MD5.Encrypt("password"));
            string host = "http://localhost:50244/api/auth";
            var request = WebRequest.Create(host);
             request.Method = "GET";
             request.ContentType = "application/json";
 
            // ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            request.Headers.Add("Authorization-Token", token);
            using (var stream = request.GetResponse())
            {
                using (var response = stream.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(response);
                    Console.Write(reader.ReadToEnd());
                }
            }
            Console.Read();
        }
    }
}
