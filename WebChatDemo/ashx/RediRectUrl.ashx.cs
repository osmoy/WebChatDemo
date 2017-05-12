using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebChat.Common;

namespace WebChatDemo.ashx
{
    /// <summary>
    /// RediRectUrl 的摘要说明
    /// </summary>
    public class RediRectUrl : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string MyAppid = CommonHelper.GetAppValue("appID");
            string RedirectUri = CommonHelper.GetAppValue("redirectUrl");
            var url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + MyAppid + "&redirect_uri=" + RedirectUri + "&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";
            context.Response.Redirect(url);
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