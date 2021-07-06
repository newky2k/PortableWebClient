using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
    public interface IEncryptionProvider
    {
        string EncryptString(string plainText, string passPhrase);

        byte[] EncryptBytes(byte[] data, string passPhrase);

        string DecryptString(string cipherText, string passPhrase);

        byte[] DecryptBytes(byte[] data, string passPhrase);
    }
}
