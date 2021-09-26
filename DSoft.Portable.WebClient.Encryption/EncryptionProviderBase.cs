using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
    /// <summary>
    /// Encryption Provider Base class for implementing a custom
    /// </summary>
    /// <seealso cref="DSoft.Portable.WebClient.Encryption.IEncryptionProvider" />
    public abstract class EncryptionProviderBase : IEncryptionProvider
    {
        protected string _initVector;

        // This constant is used to determine the keysize of the encryption algorithm.
        protected KeySize _keysize = KeySize.TwoFiftySix;

        public EncryptionProviderBase(string initVector, KeySize keySize)
        {
            _initVector = initVector;
            _keysize = keySize;
        }

        public virtual byte[] DecryptBytes(byte[] data, string passPhrase)
        {
            throw new NotImplementedException();
        }

        public virtual string DecryptString(string cipherText, string passPhrase)
        {
            throw new NotImplementedException();
        }

        public virtual byte[] EncryptBytes(byte[] data, string passPhrase)
        {
            throw new NotImplementedException();
        }

        public virtual string EncryptString(string plainText, string passPhrase)
        {
            throw new NotImplementedException();
        }

    }
}
