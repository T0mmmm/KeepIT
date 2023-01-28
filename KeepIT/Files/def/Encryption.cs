using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace KeepIT.Files.def
{
    class Encryption
    {

        public static string Encrypt(string plainText, string passPhrase)
        {
            try
            {
                int passwordIterations = 2;
                string saltValue = passPhrase;
                int keySize = 256;
                string initVector = "hadkfr9h3r9a1f4p";

                //Get the bytes
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

                //create RijndaelManaged for start work with the encryption 
                using (RijndaelManaged symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.KeySize = keySize;
                    symmetricKey.BlockSize = 128;

                    var password = new Rfc2898DeriveBytes(passPhrase, saltValueBytes, passwordIterations);
                    byte[] keyBytes = password.GetBytes(keySize / 8);
                    symmetricKey.Key = keyBytes;
                    symmetricKey.IV = initVectorBytes;

                    using (var encryptor = symmetricKey.CreateEncryptor(symmetricKey.Key, symmetricKey.IV))
                    using (var memoryStream = new MemoryStream())
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        //write the encrypt text
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();

                        byte[] cipherTextBytes = memoryStream.ToArray();

                        //return covert str in base64
                        return Convert.ToBase64String(cipherTextBytes);
                    }
                }
            }
            catch { return "Error"; }


        }

        public static string Decrypt(string cipherText, string passPhrase)
        {

            try
            {
                int passwordIterations = 2;
                string saltValue = passPhrase;
                int keySize = 256;
                string initVector = "hadkfr9h3r9a1f4p";

                //Get the bytes
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

                var key = new Rfc2898DeriveBytes(passPhrase, saltValueBytes, passwordIterations);
                byte[] keyBytes = key.GetBytes(keySize / 8);

                //start using RijndaelManaged for decryption
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                    using (var memoryStream = new MemoryStream(cipherTextBytes))
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {

                        //decrypt the text
                        var plainTextBytes = new byte[cipherTextBytes.Length];
                        int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                        //return the decrypt text
                        return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                    }
                }
            }
            catch { return "Error"; }
        }
    }
}
