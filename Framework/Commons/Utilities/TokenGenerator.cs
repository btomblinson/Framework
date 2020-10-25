using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Framework.Commons.Logging;

namespace Framework.Commons.Utilities
{
    /// <summary>
    ///     Static class that can generate tokens, compare tokens, encrypt, or decrypt values
    /// </summary>
    public static class TokenGenerator
    {
        private static string passwordHash = "AdoCxP2G7l";
        private static string saltKey = "aJ5qAkpN2w";
        private static string VectorKey = "0I2E4S6T8PlOI11s!";

        /// <summary>
        ///     Generates an encrypted value based on passed in passed in value, password hash, salt key, and vector key
        /// </summary>
        /// <param name="startOfKey">The first part of key that will be encrypted</param>
        /// <returns>Encrypted key</returns>
        public static string GenerateTokenValue(string startOfKey)
        {
            return Encrypt(startOfKey + DateTime.Now.DayOfYear + DateTime.Now.Year +
                           DateTime.Now.AddDays(2).DayOfWeek + "Brandon");
        }

        /// <summary>
        ///     Compares 2 tokens (first encryted and second is unencryted) to make sure they are the same
        /// </summary>
        /// <param name="encryptedValue">Encrypted value to compare</param>
        /// <param name="unencryptedValue">Not encrypted value that will be encrypted and compared with first value</param>
        /// <returns>True if both encrypted values are the same, false if not</returns>
        public static bool CompareTokens(string encryptedValue, string unencryptedValue)
        {
            Logger.LogInfo("eValue: " + encryptedValue + ", uValue: " + unencryptedValue);
            if (encryptedValue == GenerateTokenValue(unencryptedValue))
            {
                Logger.LogInfo("eValue: " + encryptedValue + ", GenerateduValue: " +
                               GenerateTokenValue(unencryptedValue));
                return true;
            }

            Logger.LogInfo("eValue: " + encryptedValue + ", GenerateduValue: " +
                           GenerateTokenValue(unencryptedValue));
            return false;
        }

        /// <summary>
        ///     Encrypts passed in value with password hash, salt key, and vector key.  Checks AppSettings key 'encryptionUsed'
        ///     for true/false value to determine if value sent to method truly needs encrypting
        /// </summary>
        /// <param name="value">Value to be encrypted</param>
        /// <returns>Encrypted representation of passed in parameter</returns>
        public static string Encrypt(string value)
        {
            bool valueNeedsEncryption = NeedsEncryption();

            if (!valueNeedsEncryption)
            {
                return value;
            }

            var plainTextBytes = Encoding.UTF8.GetBytes(value);

            var keyBytes = new Rfc2898DeriveBytes(passwordHash, Encoding.ASCII.GetBytes(saltKey)).GetBytes(256 / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged {Mode = CipherMode.CBC, Padding = PaddingMode.Zeros};
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VectorKey));

            byte[] cipherTextBytes;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }

                memoryStream.Close();
            }

            return Convert.ToBase64String(cipherTextBytes);
        }

        /// <summary>
        ///     Encrypts passed in value with password hash, salt key, and vector key.  Checks AppSettings key 'encryptionUsed'
        ///     for true/false value to determine if value sent to method truly needs encrypting
        /// </summary>
        /// <param name="value">Value to be encrypted</param>
        /// <returns>Encrypted representation of passed in parameter</returns>
        public static string EncryptNoOverride(string value)
        {
            return EncryptValue(value);
        }


        private static string EncryptValue(string value)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(value);

            var keyBytes = new Rfc2898DeriveBytes(passwordHash, Encoding.ASCII.GetBytes(saltKey)).GetBytes(256 / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged {Mode = CipherMode.CBC, Padding = PaddingMode.Zeros};
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VectorKey));

            byte[] cipherTextBytes;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }

                memoryStream.Close();
            }

            return Convert.ToBase64String(cipherTextBytes);
        }


        /// <summary>
        ///     Decrypts passed in value with password hash, salt key, and vector key.  Checks AppSettings key 'encryptionUsed'
        ///     for true/false value to determine if value sent to method truly needs decrypting
        /// </summary>
        /// <param name="value">Value to be decrypted</param>
        /// <returns>Decrypted representation of passed in parameter</returns>
        public static string Decrypt(string value)
        {
            bool valueNeedsEncryption = NeedsEncryption();

            return !valueNeedsEncryption ? value : DecryptString(value);
        }

        /// <summary>
        ///     Decrypts passed in value with password hash, salt key, and vector key.
        /// </summary>
        /// <param name="value">Value to be decrypted</param>
        /// <returns>Decrypted representation of passed in parameter</returns>
        private static string DecryptString(string value)
        {
            try
            {
                var cipherTextBytes = Convert.FromBase64String(value);
                int decryptedByteCount = 0;
                var plainTextBytes = new byte[cipherTextBytes.Length];
                var keyBytes =
                    new Rfc2898DeriveBytes(passwordHash, Encoding.ASCII.GetBytes(saltKey)).GetBytes(256 / 8);
                RijndaelManaged symmetricKey =
                    new RijndaelManaged {Mode = CipherMode.CBC, Padding = PaddingMode.None};

                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VectorKey));
                using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.Close();
                    }

                    memoryStream.Close();
                }

                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
            }
            catch
            {
                return value;
            }
        }

        /// <summary>
        ///     Decrypts passed in value with password hash, salt key, and vector key.  Checks AppSettings key 'encryptionUsed'
        ///     for true/false value to determine if value sent to method truly needs decrypting
        /// </summary>
        /// <param name="value">Value to be decrypted</param>
        /// <param name="isEncrypted">Tells if value is encrypted</param>
        /// <returns>Decrypted representation of passed in parameter</returns>
        public static string Decrypt(string value, bool isEncrypted)
        {
            if (!isEncrypted)
            {
                return value;
            }

            return DecryptString(value);
        }


        private static bool NeedsEncryption()
        {
            bool valueNeedsEncryption = true;
            try
            {
                if (WebConfig.Get("encryptionUsed", string.Empty) != string.Empty)
                {
                    bool.TryParse(WebConfig.Get("encryptionUsed", "false"),
                        out valueNeedsEncryption);
                }
            }
            catch
            {
                valueNeedsEncryption = true;
            }

            return valueNeedsEncryption;
        }
    }
}