using System.Web;
using System.IO;
using WebChat.Common;
using System;
using System.Text;
using WebChat.Model;

namespace WebChatDemo.ashx
{
    /// <summary>
    /// wxapi 的摘要说明
    /// </summary>
    public class wxapi : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            
            if (context.Request.HttpMethod.ToUpper() == "POST")
            {
                MenuInfo.CreateMenuPage();

                #region 处理消息
                MsgHandler msg = new MsgHandler();
                msg.ExecuteMessage(context.Request.InputStream);
                #endregion

            }
            else
            {
                ResponseServer();
            }

        }

        /// <summary>
        /// 微信接入的测试
        /// </summary>
        private void ResponseServer()
        {
            string myToken = CommonHelper.GetAppValue("myToken");
            if (string.IsNullOrEmpty(myToken))
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "error.txt", "token节点没有配置");
            }

            string echoString = HttpContext.Current.Request.QueryString["echoStr"];
            string signature = HttpContext.Current.Request.QueryString["signature"];
            string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
            string nonce = HttpContext.Current.Request.QueryString["nonce"];

            if (BasicApi.CheckSignature(myToken, signature, timestamp, nonce))
            {
                if (!string.IsNullOrEmpty(echoString))
                {
                    HttpContext.Current.Response.Write(echoString);
                    HttpContext.Current.Response.End();
                }
            }
        }

        protected string GetJson(string url)
        {
            System.Net.WebClient wc = new System.Net.WebClient();
            wc.Credentials = System.Net.CredentialCache.DefaultCredentials;
            wc.Encoding = Encoding.UTF8;
            string returnText = wc.DownloadString(url);

            if (returnText.Contains("errcode"))
            {
                //可能发生错误  
                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "errorCode.txt", "请求" + url + "出错");
            }
            //Response.Write(returnText);  
            return returnText;
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}