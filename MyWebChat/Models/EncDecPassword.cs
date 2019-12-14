using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MyWebChat.Models
{
    public class EncDecPassword
    {
        //Dekripcija
        private static string hash = "encrypt";
        public static string DecryptPassword(string password)
        {
            byte[] data = Convert.FromBase64String(password);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider
            {
                Key = keys,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform transform = tripDes.CreateDecryptor();
            byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
            password = UTF8Encoding.UTF8.GetString(results);

            return password;
        }
        //Enkripcija
        public static string EncryptPassword(string password)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(password);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider
            {
                Key = keys,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform transform = tripDes.CreateEncryptor();
            byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
            password = Convert.ToBase64String(results, 0, results.Length);

            return password;
        }
    }
}