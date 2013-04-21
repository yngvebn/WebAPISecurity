using System;
using System.Security.Cryptography;
using System.Text;

namespace RsaHelpers
{
    public class MD5
    {
        public static string Encrypt(string text)
        {
            var x = new MD5CryptoServiceProvider();
            byte[] data = Encoding.UTF8.GetBytes(text);
            data = x.ComputeHash(data);
            String md5Hash = Encoding.UTF8.GetString(data);
            return md5Hash;
        } 
    }
}