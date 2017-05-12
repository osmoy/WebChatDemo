using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using WebChat.Common;

namespace WebChatDemo
{
    public class ImageHandler
    {
        /// <summary>
        /// 下载保存多媒体文件
        /// </summary>
        /// <param name="ACCESS_TOKEN"></param>
        /// <param name="MEDIA_ID">微信服务器上的资源id</param>
        /// <returns>返回保存路径</returns>
        public static string GetMultimedia(string ACCESS_TOKEN, string MEDIA_ID)
        {
            string file = string.Empty;
            string content = string.Empty;
            string strpath = string.Empty;
            string savepath = string.Empty;
            string stUrl = "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token=" + ACCESS_TOKEN + "&media_id=" + MEDIA_ID;

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(stUrl);

            req.Method = "GET";
            using (WebResponse wr = req.GetResponse())
            {
                HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();

                strpath = myResponse.ResponseUri.ToString();
                //WriteLog("接收类别://" + myResponse.ContentType);
                WebClient mywebclient = new WebClient();
                savepath = HttpContext.Current.Server.MapPath("Image") + "\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + 
                    (new Random()).Next().ToString().Substring(0, 4) + ".jpg";
                //WriteLog("路径://" + savepath);
                try
                {
                    mywebclient.DownloadFile(strpath, savepath);
                    file = savepath;
                }
                catch (Exception ex)
                {
                    savepath = ex.ToString();
                }

            }
            return file;
        }

        // <summary>  
        /// 上传多媒体文件
        /// </summary>  
        /// <param name="ACCESS_TOKEN"></param>  
        /// <param name="Type"></param>  
        /// <returns>MediaId</returns>  
        public static string UploadMultimedia(string ACCESS_TOKEN, string Type)
        {
            string result = "";
            string wxurl = "http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token=" + ACCESS_TOKEN + "&type=" + Type;
            string filepath = HttpContext.Current.Server.MapPath("image") + "\\hemeng80.jpg";
             //WriteLog("上传路径:" + filepath);
            WebClient myWebClient = new WebClient();
            myWebClient.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                byte[] responseArray = myWebClient.UploadFile(wxurl, "POST", filepath);
                result = System.Text.Encoding.Default.GetString(responseArray, 0, responseArray.Length);
                //WriteLog("上传result:" + result);
                var _mode = JsonHelper.ParseFromJson<WebChat.Model.Wxmessage>(result);
                result = _mode.MediaId;
            }
            catch (Exception ex)
            {
                result = "Error:" + ex.Message;
            }
            //WriteLog("上传MediaId:" + result);
            return result;
        }

      



    }
}