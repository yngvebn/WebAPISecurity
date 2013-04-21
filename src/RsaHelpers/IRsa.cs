namespace RsaHelpers
{
    public interface IRsa
    {
        string Decrypt(string data);
        string Encrypt(string data);
    }
}