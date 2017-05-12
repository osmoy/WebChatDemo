using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using WebChat.Common;
using WebChar.Model;

namespace WebChatDemo
{
    public class QRCodeHandler
    {
        /// <summary>
        /// 创建二维码ticket
        /// </summary>
        /// <param name="TOKEN"></param>
        /// <returns></returns>
        public static string CreateTicket(string TOKEN)
        {
            string result = "";
            //string strJson = @"{""expire_seconds"":1800, ""action_name"": ""QR_SCENE"", ""action_info"": {""scene"": {""scene_id"":100000023}}}";  
            string strJson = @"{""action_name"": ""QR_LIMIT_SCENE"", ""action_info"": {""scene"": {""scene_id"":100000024}}}";
            string wxurl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + TOKEN;

            WebClient myWebClient = new WebClient();
            myWebClient.Credentials = CredentialCache.DefaultCredentials;
            try
            {

                result = myWebClient.UploadString(wxurl, "POST", strJson);
                //WriteLog("上传result:" + result);  
                Ticket _mode = JsonHelper.ParseFromJson<Ticket>(result);               
                result = _mode.ticket + "_" + _mode.expire_seconds;
            }
            catch (Exception ex)
            {
                result = "Error:" + ex.Message;
            }
            //WriteLog("上传MediaId:" + result);  

            return result;
        }

        /// <summary>
        /// 根据Ticket创建图片
        /// </summary>
        public static string GetImgByTicket(string ticket)
        {
            string content = string.Empty;
            string strpath = string.Empty;
            string savepath = string.Empty;
            var context = HttpContext.Current;

            string stUrl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + context.Server.UrlEncode(ticket);

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(stUrl);

            req.Method = "GET";

            using (WebResponse wr = req.GetResponse())
            {
                HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();
                strpath = myResponse.ResponseUri.ToString();

                WebClient mywebclient = new WebClient();

                savepath = context.Server.MapPath("image") + "\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + (new Random()).Next().ToString().Substring(0, 4) + "." + myResponse.ContentType.Split('/')[1].ToString();

                try
                {
                    mywebclient.DownloadFile(strpath, savepath);
                }
                catch (Exception ex)
                {
                    savepath = ex.ToString();
                }
            }
            return strpath.ToString();
        }  


    }
}