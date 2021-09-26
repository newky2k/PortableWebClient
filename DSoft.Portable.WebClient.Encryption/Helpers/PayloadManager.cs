using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DSoft.Portable.WebClient.Encryption.Helpers
{
    public class PayloadManager
    {
        private static TimeSpan _defaultTimeSpan = TimeSpan.FromMinutes(10);
        public static TimeSpan DefaultTimeout
        {
            get
            {
                return _defaultTimeSpan;
            }
            set
            {
                _defaultTimeSpan = value;
            }
        }

        /// <summary>
        /// Decrypt the specified payload
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="payload">Payload string</param>
        /// <param name="passkey">Encryption key</param>
        /// <returns></returns>
        public static T DecryptPayload<T>(string payload, string passkey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
        {
            var decryptedPayload = EncryptionProviderFactory.Build(initVector, keySize).DecryptString(payload, passkey);

            var outPut = JsonSerializer.Deserialize<T>(decryptedPayload);

            return outPut;
        }

        /// <summary>
        /// Encrypt the specified payload
        /// </summary>
        /// <param name="payload">Payload object</param>
        /// <param name="passkey">Encryption key</param>
        /// <returns></returns>
        public static string EncryptPayload(object payload, string passkey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
        {
            var payloadString = JsonSerializer.Serialize(payload, payload.GetType());

            return EncryptionProviderFactory.Build(initVector, keySize).EncryptString(payloadString, passkey);
        }



    }
}
