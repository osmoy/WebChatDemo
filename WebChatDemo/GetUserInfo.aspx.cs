using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebChat.Common;
using WebChat.Model;
using System.Text;

namespace WebChatDemo
{
    public partial class GetUserInfo : System.Web.UI.Page
    {
        protected string appId = CommonHelper.GetAppValue("appID");
        protected string secret = CommonHelper.GetAppValue("appsecret");
        protected string url = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            var code = Request.QueryString["code"];
            if (!string.IsNullOrEmpty(code))
            {
                var model = Get_token(code);//作缓存 处理
                //model = Refresh_token(model.refresh_token);
                OAuthUser OAuthUser_Model = Get_UserInfo(model.access_token, model.openid);//存入数据库
                StringBuilder sb = new StringBuilder();
                sb.Append("用户OPENID:" + OAuthUser_Model.openid + "<br>");
                sb.Append("用户昵称:" + OAuthUser_Model.nickname + "<br>");
                sb.Append("性别:" + OAuthUser_Model.sex == "1" ? "男" : (OAuthUser_Model.sex == "2" ? "女" : "未知") + "<br>");
                sb.Append("所在省:" + OAuthUser_Model.province + "<br>");
                sb.Append("所在市:" + OAuthUser_Model.city + "<br>");
                sb.Append("所在国家:" + OAuthUser_Model.country + "<br>");
                sb.Append("头像地址:" + OAuthUser_Model.headimgurl);
                Response.Write(sb.ToString());

                #region 展示二维码
                //string ticket = QRCodeHandler.CreateTicket(PartenerInfo.IsTokenExpired());
                //ticket = ticket.Split('_')[0];
                //url = QRCodeHandler.GetImgByTicket(ticket);                
                #endregion

            }

        }

        /// <summary>
        /// 通过code换取网页授权access_token
        /// </summary>
        private OAuth_Token Get_token(string code)
        {
            var url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";
            url = string.Format(url, appId, secret, code);
            string res = GetJson(url);
            OAuth_Token Oauth_Token_Model = JsonHelper.ParseFromJson<OAuth_Token>(res);
            return Oauth_Token_Model;
        }

        /// <summary>
        /// 刷新Token
        /// </summary>  
        private OAuth_Token Refresh_token(string refresh_token)
        {
            string res = GetJson("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid=" + appId + "&grant_type=refresh_token&refresh_token=" + refresh_token);
            OAuth_Token Oauth_Token_Model = JsonHelper.ParseFromJson<OAuth_Token>(res);
            return Oauth_Token_Model;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private OAuthUser Get_UserInfo(string access_token, string oppenid)
        {
            var res = GetJson("https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + oppenid + "&lang=zh_CN");
            OAuthUser OAuthUser_Model = JsonHelper.ParseFromJson<OAuthUser>(res);
            return OAuthUser_Model;
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="url"></param>        
        protected string GetJson(string url)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
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


    }
}