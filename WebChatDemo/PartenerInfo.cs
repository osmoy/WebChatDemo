using System;
using System.Net;
using System.Web;
using System.IO;
using System.Text;
using WebChat.Model;
using System.Xml;
using WebChat.Common;

namespace WebChatDemo
{
    public class PartenerInfo
    {
        private static readonly string appId = CommonHelper.GetAppValue("appID");
        private static readonly string appSecret = CommonHelper.GetAppValue("appsecret");

        /// <summary>
        /// 根据当前日期 判断Access_Token 是否过期  如果过期重新请求
        /// 否则返回之前的Access_Token  
        /// </summary>        
        public static string IsTokenExpired()
        {
            string Token = string.Empty;
            DateTime YouXRQ;
            // 读取XML文件中的数据，并显示出来 ，注意文件路径
            string filepath = HttpContext.Current.Server.MapPath("/Token_Xml/XMLFile.xml");

            StreamReader str = new StreamReader(filepath, Encoding.UTF8);
            XmlDocument xml = new XmlDocument();
            xml.Load(str);
            str.Close();
            str.Dispose();
            Token = xml.SelectSingleNode("xml").SelectSingleNode("Access_Token").InnerText;
            YouXRQ = Convert.ToDateTime(xml.SelectSingleNode("xml").SelectSingleNode("Access_Expir").InnerText);

            if (DateTime.Now > YouXRQ)
            {
                DateTime _youxrq = DateTime.Now;
                AccessToken mode = GetToken();
                //修改xml文件
                xml.SelectSingleNode("xml").SelectSingleNode("Access_Token").InnerText = mode.access_token;
                _youxrq = _youxrq.AddSeconds(int.Parse(mode.expires_in));
                xml.SelectSingleNode("xml").SelectSingleNode("Access_Expir").InnerText = _youxrq.ToString();
                xml.Save(filepath);
                Token = mode.access_token;
            }
            return Token;
        }

        /// <summary>
        /// 获取全局唯一票据
        /// </summary>        
        private static AccessToken GetToken()
        {
            var url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appId + "&secret=" + appSecret;
            var strJson = RequestUrl(url, "GET");
            var access_token = CommonHelper.GetJsonValue(strJson, "access_token");
            var expires_in = CommonHelper.GetJsonValue(strJson, "expires_in");
            var model = new AccessToken() { access_token = access_token, expires_in = expires_in };
            #region 第二种方式
            //AccessToken model = new AccessToken();
            //model = JsonHelper.ParseFromJson<AccessToken>(strJson);
            #endregion

            return model;
        }

        /// <summary>
        /// 验证Token是否过期
        ///【获取自定义菜单，如果返回42001，则说明access_token已失效】
        /// </summary>
        private static bool IsTokenExpired(string access_token)
        {
            string jsonStr = RequestUrl(string.Format("https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}", access_token), "GET");
            if (CommonHelper.GetJsonValue(jsonStr, "errcode") == "42001")
            {
                return true;////缓存或者数据库
            }
            return false;
        }

        /// <summary>
        /// 请求Url
        /// </summary>
        public static string RequestUrl(string url, string method)
        {
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = method;
            request.ContentType = "text/html";
            request.Headers.Add("charset", "utf-8");

            //发送请求并获取相应回应数据
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream responseStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(responseStream, Encoding.UTF8);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                return content;
            }                      
        }

        #region MyRegion
        ///// <summary>
        ///// 获取access_token
        ///// </summary>
        //public static string GetAccessToken(PageBase page)
        //{
        //    string access_token = string.Empty;

        //    UserInfo user = GetLoginUser(page);
        //    if (user != null)
        //    {
        //        if (string.IsNullOrWhiteSpace(user.access_token)) //尚未保存过access_token
        //        {
        //            access_token = GetToken(user.AppID, user.AppSecret);
        //        }
        //        else
        //        {
        //            if (TokenExpired(user.access_token)) //access_token过期
        //            {
        //                access_token = GetToken(user.AppID, user.AppSecret);
        //            }
        //            else
        //            {
        //                return user.access_token;
        //            }
        //        }

        //        MSSQLHelper.ExecuteSql(string.Format("update SWX_Config set access_token='{0}' where UserName='{1}'", access_token, user.UserName));
        //    }

        //    return access_token;
        //}
        #endregion

        /// <summary>
        /// 获取响应内容
        /// </summary>
        /// <param name="posturl">请求的url</param>
        /// <param name="postData">请求数据</param>
        /// <returns></returns>
        public static string GetPage(string posturl, string postData)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...  
            try
            {
                // 设置参数  
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据  
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求  
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码  
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                HttpContext.Current.Response.Write(err);
                return string.Empty;
            }
        }


    }
}