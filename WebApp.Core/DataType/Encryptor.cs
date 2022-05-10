using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebApp.Core.DataType
{
    public static class Encryptor
    {
        private const string Key = "123.key";

        private const int SaltSize = 32;

        public static byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[SaltSize];

                rng.GetBytes(randomNumber);

                return randomNumber;

            }
        }

        public static HashAlgorithm HashProvider(string hashAlgorithm = "md5")
        {
            var hashProvider = HashAlgorithm.Create(hashAlgorithm.ToUpper());
            //switch (hashAlgorithm.ToUpper())
            //{
            //    case "SHA384":
            //        hashProvider = new SHA384Managed();
            //        break;

            //    case "SHA512":
            //        hashProvider = new SHA512Managed();
            //        break;

            //    default:
            //        hashProvider = new MD5CryptoServiceProvider();
            //        break;
            //}

            return hashProvider;
        }

        public static string Encrypt(this string str)
        {
            try
            {
                return Encrypt(str, Key);
            }

            catch (Exception ex)
            {
                return "Wrong Input. " + ex.Message;
            }
        }

        public static string Decrypt(this string str)
        {
            try
            {
                return Decrypt(str, Key);
            }
            catch (Exception ex)
            {
                return "Wrong Input. " + ex.Message;
            }
        }

        public static string Encrypt(string str, string key, string hashAlgorithm = "md5")
        {
            var hashProvider = HashProvider(hashAlgorithm);
            var desProvider = new TripleDESCryptoServiceProvider();

            try
            {
                desProvider.Mode = CipherMode.ECB;
                desProvider.Key = hashProvider.ComputeHash(Encoding.ASCII.GetBytes(key));
                desProvider.Padding = PaddingMode.PKCS7;

                var bytes = Encoding.ASCII.GetBytes(str);

                return Convert.ToBase64String(desProvider.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length));
            }
            catch (Exception ex)
            {
                return "Wrong Input. " + ex.Message;
            }
            finally
            {
                hashProvider.Clear();
                desProvider.Clear();
            }
        }

        public static string Decrypt(string str, string key, string hashAlgorithm = "md5")
        {
            var hashProvider = HashProvider(hashAlgorithm);
            var desProvider = new TripleDESCryptoServiceProvider();

            try
            {
                desProvider.Mode = CipherMode.ECB;
                desProvider.Key = hashProvider.ComputeHash(Encoding.ASCII.GetBytes(key));
                desProvider.Padding = PaddingMode.PKCS7;

                var bytes = Convert.FromBase64String(str);

                return Encoding.ASCII.GetString(desProvider.CreateDecryptor().TransformFinalBlock(bytes, 0, bytes.Length));
            }
            catch (Exception ex)
            {
                return "Wrong Input. " + ex.Message;
            }
            finally
            {
                hashProvider.Clear();
                desProvider.Clear();
            }
        }

        public static bool VerifyHash(string plainText, string hashValue, string hashAlgorithm)
        {
            var decrypt = Decrypt(hashValue, Key, hashAlgorithm);

            return plainText.Equals(decrypt);
        }
    }
}