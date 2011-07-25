using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace PockItKnife
{
    /// <summary>
    /// Provides functionality to easily De- or Encrypt strings. Is not safe, but can be used to avoid plaintext passwords in config files.
    /// </summary> 
    public class Crypt
    {
        private string _forCrypto;
        internal Crypt(string forCrypt)
        {
            _forCrypto = forCrypt;
        }

        /// <summary>
        /// Decrypt the string using a password.
        /// </summary>
        /// <param name="password">Password, used to decrypt the string.</param>
        /// <returns></returns>
        public string De(string password)
        { 
            if (password == null)
                throw new ArgumentNullException("cyperSeed may not be null");
            if (password.Length < 8)
                throw new ArgumentException("cyperSeed must be longer that 7 characters");

            try {
                return this.DecryptString(_forCrypto, password);
            }
            catch (System.FormatException ex){
                if (ex.Message.Contains("Invalid length for a Base-64 char array"))
                    return _forCrypto;
                throw;
            }
        }

        /// <summary>
        /// Encrypt the string using a password.
        /// </summary>
        /// <param name="password">Password, used to encrypt the string.</param>
        /// <returns></returns>
        public string En(string password)
        {
            if (password == null)
                throw new ArgumentNullException("cyperSeed may not be null");
            if (password.Length < 8)
                throw new ArgumentException("cyperSeed must be longer that 7 characters");
            return this.EncryptString(_forCrypto, password);
        }

        private byte[] EncryptString(byte[] clearText, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = Key;
            alg.IV = IV;
            CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(clearText, 0, clearText.Length);
            cs.Close();
            byte[] encryptedData = ms.ToArray();
            return encryptedData;
        }

        /// <summary>
        /// Encrypts the string.
        /// </summary>
        /// <param name="clearText">The clear text.</param>
        /// <param name="Password">The password.</param>
        /// <returns></returns>
        private string EncryptString(string clearText, string Password)
        {
            if (string.IsNullOrEmpty(clearText))
                return clearText;

            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            byte[] encryptedData = EncryptString(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// Decrypts the string.
        /// </summary>
        /// <param name="cipherData">The cipher data.</param>
        /// <param name="Key">The key.</param>
        /// <param name="IV">The IV.</param>
        /// <returns></returns>
        private byte[] DecryptString(byte[] cipherData, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = Key;
            alg.IV = IV;
            CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(cipherData, 0, cipherData.Length);
            cs.Close();
            byte[] decryptedData = ms.ToArray();
            return decryptedData;
        }

        /// <summary>
        /// Decrypts the string.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="Password">The password.</param>
        /// <returns></returns>
        private string DecryptString(string cipherText, string Password)
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;

            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            byte[] decryptedData = DecryptString(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return System.Text.Encoding.Unicode.GetString(decryptedData);
        }
    }
}
