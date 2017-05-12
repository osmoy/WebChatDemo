using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebChat.Common
{
    public class SHA1Helper
    {       
        /// <summary>
        /// 获取由MD5加密的字符串
        /// </summary>
        public static string EncryptToMD5(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] str1 = Encoding.UTF8.GetBytes(str);
            byte[] str2 = md5.ComputeHash(str1, 0, str1.Length);
            md5.Clear();
            (md5 as IDisposable).Dispose();
            return Convert.ToBase64String(str2);
        }

        #region [ 生成hmacsha1的散列 ]
        public static string HmacSha1(string word)
        {
            return BitConverter.ToString(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(word))).Replace("-", string.Empty);
        }
        #endregion

        #region  [ 字符串的字典序排序,区分大小写 ]
        public static string GetOrder(string[] ss)
        {
            var list = ss.OrderBy(x => x, StringComparer.Ordinal).ToArray();
            return string.Join("", list);
        }
        #endregion

    }  
}
