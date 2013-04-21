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

        private static string publicToken =
            "<RSAKeyValue><Modulus>s6lpjspk+3o2GOK5TM7JySARhhxE5gB96e9XLSSRuWY2W9F951MfistKRzVtg0cjJTdSk5mnWAVHLfKOEqp8PszpJx9z4IaRCwQ937KJmn2/2VyjcUsCsor+fdbIHOiJpaxBlsuI9N++4MgF/jb0tOVudiUutDqqDut7rhrB/oc=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        private static RsaEncrypter _rsaEncrypter;

        static void Main(string[] args)
        {
             _rsaEncrypter = new RsaEncrypter(publicKey: publicToken);
            var token = _rsaEncrypter.Encrypt("yngvebn");
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
