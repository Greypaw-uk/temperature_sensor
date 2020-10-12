using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TempMonitoring
{
    public class Crypto
    {
        //Key and vector used for the encoding process.
        private static byte[] key = { 65, 202, 99, 123, 25, 96, 185, 185, 24, 58, 134, 147, 47, 211, 16, 139, 1, 98, 206, 39, 87, 16, 28, 111, 162, 248, 65, 128, 164, 69, 71, 21 };
        private static byte[] vector = { 6, 20, 7, 17, 254, 201, 189, 167, 83, 59, 86, 116, 101, 34, 176, 250 };

        /// <summary>
        /// Encode plain text and return a base64 encoded string.
        /// </summary>
        /// <param name="plainText">String to be encoded</param>
        /// <returns>Encoded base64 string</returns>
        public static string EncryptString(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return "";

            RijndaelManaged rm = new RijndaelManaged();
            rm.Padding = PaddingMode.PKCS7;
            ICryptoTransform encryptor = rm.CreateEncryptor(key, vector);
            UTF8Encoding encoder = new UTF8Encoding();

            //Translates our text value into a byte array.
            Byte[] bytes = encoder.GetBytes(plainText);

            //Used to stream the data in and out of the CryptoStream.
            MemoryStream memoryStream = new MemoryStream();

            #region Write the decrypted value to the encryption stream
            CryptoStream cs = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cs.Write(bytes, 0, bytes.Length);
            cs.FlushFinalBlock();
            #endregion

            #region Read encrypted value back out of the stream
            memoryStream.Position = 0;
            byte[] encrypted = new byte[memoryStream.Length];
            memoryStream.Read(encrypted, 0, encrypted.Length);
            #endregion

            //Clean up.
            cs.Close();
            memoryStream.Close();

            return Convert.ToBase64String(encrypted);
        }

        /// <summary>
        /// Decode cipher into a plain text string.
        /// </summary>
        /// <param name="cipherText">Encoded base64 value</param>
        /// <returns>Plain text value as string</returns> 
        public static string DecryptString(string cipherText)
        {
            try
            {
                if (string.IsNullOrEmpty(cipherText))
                    return "";

                RijndaelManaged rm = new RijndaelManaged();
                rm.Padding = PaddingMode.PKCS7;
                ICryptoTransform DecryptorTransform = rm.CreateDecryptor(key, vector);
                UTF8Encoding encoder = new UTF8Encoding();

                byte[] EncryptedValue = Convert.FromBase64String(cipherText);

                #region Write the encrypted value to the decryption stream
                MemoryStream encryptedStream = new MemoryStream();
                CryptoStream decryptStream = new CryptoStream(encryptedStream, DecryptorTransform, CryptoStreamMode.Write);
                decryptStream.Write(EncryptedValue, 0, EncryptedValue.Length);
                decryptStream.FlushFinalBlock();
                #endregion

                #region Read the decrypted value from the stream.
                encryptedStream.Position = 0;
                Byte[] decryptedBytes = new Byte[encryptedStream.Length];
                encryptedStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                encryptedStream.Close();
                #endregion

                return encoder.GetString(decryptedBytes);
            }
            catch
            {
                return "";
            }
        }
    }
}
