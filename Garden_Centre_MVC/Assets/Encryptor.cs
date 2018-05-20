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
        /// This methiod will take a password and return a newly encrpyted password along with its salt as a byte array to be stored in the database
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

        /// <summary>
        /// this will take the password and salt from the db and also the password entered by
        /// the user and then return a boolean which will say if it was or wasnt a match
        /// </summary>
        /// <param name="passwordEntered"></param>
        /// <param name="passwordTocheck"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static bool Check(string passwordEntered, byte[] passwordTocheck, byte[] salt)
        {
            var provider = new Rfc2898DeriveBytes(passwordEntered, salt, _noOfIterations);

            var one = GetString(provider.GetBytes(32));

            var two = GetString(passwordTocheck);

            return one == two;
        }


        #region Private Methods
        /// <summary>
        /// this method will get the string from the byte array
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static string GetString(byte[] bytes)
        {
            return Encoding.Default.GetString(bytes);
        }

        /// <summary>
        /// this method will generate a random salt and return it as a array of bytes.
        /// </summary>
        /// <returns></returns>
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