using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebApp.Core.DataType
{
    public static class Hmac
    {
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

        public static byte[] ComputeHmacSha256(byte[] str, byte[] salt)
        {

            using (var hmac = new HMACSHA256(salt))
            {
                return hmac.ComputeHash(str);
            }
        }

        public static string ComputeHmacSha256(string str, string salt)
        {
            var data = Encoding.ASCII.GetBytes(str);
            var key = Encoding.ASCII.GetBytes(salt);

            return Encoding.ASCII.GetString(ComputeHmacSha256(data, key));
        }

        //var salt = Hmac.GenerateSalt();

        //var hmac1 = Hmac.ComputeHmacSha256(Encoding.UTF8.GetBytes(orgMsg), salt);
        //var hmac2 = Hmac.ComputeHmacSha256(Encoding.UTF8.GetBytes(otherMsg), salt);
    }
}