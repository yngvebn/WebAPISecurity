using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RsaHelpers
{
    public class Rsa : IRsa
    {
        private readonly IRsaKeys _rsaKeys;
        private static readonly UnicodeEncoding Encoder = new UnicodeEncoding();

        public Rsa(IRsaKeys rsaKeys)
        {
            _rsaKeys = rsaKeys;
        }

        public string Decrypt(string data)
        {
            var rsa = new RSACryptoServiceProvider();
            byte[] dataByte = FromHex(data);
            rsa.FromXmlString(_rsaKeys.PrivateKey);
            var decryptedByte = rsa.Decrypt(dataByte, false);
            return Encoder.GetString(decryptedByte);
        }

        public string Encrypt(string data)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(_rsaKeys.PublicKey);
            var dataToEncrypt = Encoder.GetBytes(data);
            var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false);

            return ToHex(encryptedByteArray);
        }

        private static string ToHex(IEnumerable<byte> bytes)
        {
            string result = "";
            foreach (byte b in bytes)
            {
                result = result + Convert.ToString(b, 16).PadLeft(2, '0');
            }
            return result;
        }

        private static byte[] FromHex(string hexString)
        {
            var hexPair = "";
            var bytes = new List<byte>();
            for (var i = 0; i < hexString.Length; i++)
            {
                hexPair += hexString[i];
                if (i%2 == 0) continue;
                bytes.Add(Convert.ToByte(hexPair, 16));
                hexPair = "";
            }
            return bytes.ToArray();
        }
    }
}