using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebChat.Common;
using System.Web.Security;

namespace WebChatDemo
{
    public class BasicApi
    {
        private static BasicApi api = null;

        static BasicApi()
        {
            api = new BasicApi();
        }

        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// <param name="token">此token不是全局access_token</param>       
        public static bool CheckSignature(string token, string signature, string timestamp, string nonce)
        {
            string[] ArrTmp = { token, timestamp, nonce };

            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);

            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1"); 
            tmpStr = tmpStr.ToLower();

            if (tmpStr.Equals(signature))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}