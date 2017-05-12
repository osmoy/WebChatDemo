using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebChatDemo
{
    public partial class SendMessage : System.Web.UI.Page
    {
        string MyOpenID;
        string MyContent;
        protected void Page_Load(object sender, EventArgs e)
        {
            MyOpenID = "oV93gjl5slD3p29yS1dOijy-pqZ8";
            MyContent = "这是一个客服消息";
            UserName.Text = MyOpenID;
            txtMsg.Text = MyContent;  
        }

        protected void btngo_Click(object sender, EventArgs e)
        {
            string res = "";
            string access_token = PartenerInfo.IsTokenExpired();

            string posturl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + access_token;
            string postData = "{\"touser\":\"" + UserName.Text + "\",\"msgtype\":\"text\",\"text\":{\"content\":\"" + txtMsg.Text + "\"}}";
            res = PartenerInfo.GetPage(posturl, postData);

            Response.Write(res);  
        }

    }
}