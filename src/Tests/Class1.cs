using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RsaHelpers;

namespace Tests
{
    [TestFixture]
    public class RsaTests
    {
        private static string _privateKey =
           "<RSAKeyValue><Modulus>s6lpjspk+3o2GOK5TM7JySARhhxE5gB96e9XLSSRuWY2W9F951MfistKRzVtg0cjJTdSk5mnWAVHLfKOEqp8PszpJx9z4IaRCwQ937KJmn2/2VyjcUsCsor+fdbIHOiJpaxBlsuI9N++4MgF/jb0tOVudiUutDqqDut7rhrB/oc=</Modulus><Exponent>AQAB</Exponent><P>3J2+VWMVWcuLjjnLULe5TmSN7ts0n/TPJqe+bg9avuewu1rDsz+OBfP66/+rpYMs5+JolDceZSiOT+ACW2Neuw==</P><Q>0HogL5BnWjj9BlfpILQt8ajJnBHYrCiPaJ4npghdD5n/JYV8BNOiOP1T7u1xmvtr2U4mMObE17rZjNOTa1rQpQ==</Q><DP>jbXh2dVQlKJznUMwf0PUiy96IDC8R/cnzQu4/ddtEe2fj2lJBe3QG7DRwCA1sJZnFPhQ9svFAXOgnlwlB3D4Gw==</DP><DQ>evrP6b8BeNONTySkvUoMoDW1WH+elVAH6OsC8IqWexGY1YV8t0wwsfWegZ9IGOifojzbgpVfIPN0SgK1P+r+kQ==</DQ><InverseQ>LeEoFGI+IOY/J+9SjCPKAKduP280epOTeSKxs115gW1b9CP4glavkUcfQTzkTPe2t21kl1OrnvXEe5Wrzkk8rA==</InverseQ><D>HD0rn0sGtlROPnkcgQsbwmYs+vRki/ZV1DhPboQJ96cuMh5qeLqjAZDUev7V2MWMq6PXceW73OTvfDRcymhLoNvobE4Ekiwc87+TwzS3811mOmt5DJya9SliqU/ro+iEicjO4v3nC+HujdpDh9CVXfUAWebKnd7Vo5p6LwC9nIk=</D></RSAKeyValue>";

        private static string _publicKey =
            "<RSAKeyValue><Modulus>s6lpjspk+3o2GOK5TM7JySARhhxE5gB96e9XLSSRuWY2W9F951MfistKRzVtg0cjJTdSk5mnWAVHLfKOEqp8PszpJx9z4IaRCwQ937KJmn2/2VyjcUsCsor+fdbIHOiJpaxBlsuI9N++4MgF/jb0tOVudiUutDqqDut7rhrB/oc=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";


        internal class HardCodedRsaKeys : IRsaKeys
        {
            public string PublicKey
            {
                get
                {
                    return
                        "<RSAKeyValue><Modulus>1TGTGmcJRlLCRXo9M38P9LDWIkBXEbKHefxQru13votIB0ODnlKPK2GOpEfJcr66pnjRdON/xcvEojKLjOFC1l1lCzrwTnCD8//aY8YmTgoxT6K4ytJrlUuEusnMzPJuZviYUSl2Takbe0vKilaztJi0nj8HlJFDC8Vu/JF/S0k=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
                }
            }
            public string PrivateKey
            {
                get
                {
                    return
                        "<RSAKeyValue><Modulus>1TGTGmcJRlLCRXo9M38P9LDWIkBXEbKHefxQru13votIB0ODnlKPK2GOpEfJcr66pnjRdON/xcvEojKLjOFC1l1lCzrwTnCD8//aY8YmTgoxT6K4ytJrlUuEusnMzPJuZviYUSl2Takbe0vKilaztJi0nj8HlJFDC8Vu/JF/S0k=</Modulus><Exponent>AQAB</Exponent><P>7WcuuUMEOOB6AI9POJQCHTdqj0pvuMlI1BI/N7DSCCO0fJdevJrBfUtfbSxWQELsGL5y3EzcKoeop3YugbCg1Q==</P><Q>5eTnNMvFsPxqW+FrlB6vcDFZ9xPIkh2sPgJNZjiQKh6H7E/8Az2e/CMWdwigHA8qzNvaEPurhKEC2q6+ugoapQ==</Q><DP>kUurTvtzJBRO1vTeuXPsb1ExSI14HxIiHpkkU8NGaHDhz7cc5jWY4kQ1HS4bg6zxrpsw1R+9R9JLKGKuR/WAGQ==</DP><DQ>OsN4FhbAQa1DwpisVwBA9/ylcnKsIi1TicYs4qQytZF4TP9k+68UpH6Tj3m083ctCZBo/U5XWV+Oyzc/qW5LwQ==</DQ><InverseQ>rg/rBMmudXUIBOyN+vd26HFhuNXwMk0WxLsW/cDElufRcN+ieNiqwy3V5g5Kak4SoiHCFr4+pONyRc+AbYfJgw==</InverseQ><D>W4se/E5UCDNPIiA8GVmtE0e/mxN/j6TWUYYLayGisloCQsQ1xwzyVxFb+6Srlq7ZXNQyNHvfiKJXu8HydDrhxHvL/cDWcV6Ua72iiCEdCIv4XOt+5L52L4qaZekIjPUZwejTduuhGZDIPbLd4p23vsyCN3rfi3unEcCzE2Sjs5E=</D></RSAKeyValue>";
                }
            }
        }
     
        [Test]
        public void CanEncryptAndDecryptAstring()
        {
            var rsaKeysMock = new Mock<IRsaKeys>();
            rsaKeysMock.Setup(r => r.PublicKey).Returns(_publicKey);
            rsaKeysMock.Setup(r => r.PrivateKey).Returns(_privateKey);

            string toEncrypt = "yngve";
            Rsa rsa = new Rsa(new HardCodedRsaKeys());
            var encrypted = rsa.Encrypt(toEncrypt);
            Assert.That(rsa.Decrypt(encrypted), Is.EqualTo(toEncrypt));
        }
    }
}
