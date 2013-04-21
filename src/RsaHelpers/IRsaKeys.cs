using Ninject.extensions.DictionaryAdapter;

namespace RsaHelpers
{
    [KeyPrefix(Prefix="Rsa.")]
    public interface IRsaKeys
    {
        string PublicKey { get; }
        string PrivateKey { get;  }
    }
}