using System;
using System.IO;
using System.Security.Cryptography;

// https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=netcore-3.1
namespace MQTTCloud.MQTT
{
    public class AESCryptography
    {
        public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.KeySize = 128;
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] key, byte[] iv)
        {
            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.KeySize = 128;
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        public static byte[] GenerateKey()
        {
            using Aes aesAlg = Aes.Create();
            if (aesAlg != null)
            {
                aesAlg.KeySize = 128;
                aesAlg.GenerateKey();
                return aesAlg.Key;
            }

            return null;
        }
        
        public static byte[] GenerateIV()
        {
            using Aes aesAlg = Aes.Create();
            if (aesAlg != null)
            {
                aesAlg.KeySize = 128;
                aesAlg.GenerateIV();
                return aesAlg.IV;
            }

            return null;
        }
    }
}
