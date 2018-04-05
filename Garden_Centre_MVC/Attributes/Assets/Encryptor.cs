using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Garden_Centre_MVC.Assets
{
    public static class Encryptor
    {
        private static int _noOfIterations = 10;

        /// <summary>
        /// This method returns a list with the following indexes
        /// [0] == password
        /// [1] == salt
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static List<byte[]> Encrypt(string password)
        {
            var salt = GenerateSalt();

            var rfc2 = new Rfc2898DeriveBytes(password, salt, _noOfIterations);

            List<byte[]> details = new List<byte[]>()
            {
                rfc2.GetBytes(32),
                salt
            };

            return details;

        }

        public static bool Check(string passwordEntered, byte[] passwordTocheck, byte[] salt)
        {
            var provider = new Rfc2898DeriveBytes(passwordEntered, salt, _noOfIterations);

            var one = GetString(provider.GetBytes(32));

            var two = GetString(passwordTocheck);

            return one == two;
        }


        #region Private Methods
        private static string GetString(byte[] bytes)
        {
            return Encoding.Default.GetString(bytes);
        }

        private static byte[] GenerateSalt()
        {
            var salt = new byte[32];

            var provider = new RNGCryptoServiceProvider();

            provider.GetBytes(salt);

            return salt;
        }

        #endregion
    }
}