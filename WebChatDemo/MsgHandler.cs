using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebChat.Model;
using System.IO;
using System.Xml;
using WebChat.Common;

namespace WebChatDemo
{
    public class MsgHandler
    {
        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="inputStream"></param>
        public void ExecuteMessage(Stream inputStream)
        {
            Wxmessage wx = GetWxMessage(inputStream);
            LogHelper.WriteLog(wx.FromUserName);//记录用户的OPENID
            string res = string.Empty;

            if (!string.IsNullOrEmpty(wx.EventName) && wx.EventName.Trim() == "subscribe")
            {//刚关注时，用于欢迎词  
                string content = string.Empty;
                if (!wx.EventKey.Contains("qrscene_"))
                {
                    content = "/:rose欢迎xx科技有限公司/:rose\n直接回复“你好”";
                    res = sendTextMessage(wx, content);
                }
                else
                {
                    content = "二维码参数：\n" + wx.EventKey.Replace("qrscene_", "");
                    res = sendTextMessage(wx, content);  
                }
            }
            else if (!string.IsNullOrEmpty(wx.EventName) && wx.EventName.ToLower() == "scan")
            {
                string str = "二维码参数：\n" + wx.EventKey;
                res = sendTextMessage(wx, str);
            }
            else if (!string.IsNullOrEmpty(wx.EventName) && wx.EventName.Trim() == "CLICK")
            {
                if (wx.EventKey == "V1001_GOOD")
                    res = sendTextMessage(wx, "你好,谢谢支持，请继续关注我们!");
                if (wx.EventKey == "today")
                    res = sendPicTextMessage(wx, "瑞典首都一卡车冲撞人群 造成多人死伤", "描述仔细", "http://p3.pstatp.com/origin/1a6b000c52144a2ccf47", "http://www.toutiao.com/a6406254221851787522/#p=1");

            }
            else if (!string.IsNullOrEmpty(wx.EventName) && wx.EventName.Trim() == "scancode_waitmsg")
            {
                if (wx.EventKey == "rselfmenu_0_0")
                    res = sendTextMessage(wx, "恭喜，您中了500W大奖！");
            }
            else if (!string.IsNullOrEmpty(wx.EventName) && wx.EventName.Trim() == "LOCATION")//获取用户地理位置..
            {
                res = sendTextMessage(wx, "您的位置是经度：" + wx.Latitude + "，维度是：" + wx.Longitude + "，地理经度为：" + wx.Precision);
            }
            else if (!string.IsNullOrEmpty(wx.EventName) && wx.EventName.Trim() == "image")//多媒体
            {
                res = ImageHandler.GetMultimedia(PartenerInfo.IsTokenExpired(), wx.MediaId);
            }
            else
            {
                if (wx.MsgType == "text" && wx.Content == "你好")
                {
                    res = sendTextMessage(wx, "你好,欢迎使用xx科技有限公司公共微信平台!");
                }
                else if (wx.MsgType == "text" && wx.Content == "授权")
                {
                    string MyAppid = CommonHelper.GetAppValue("appID");
                    string RedirectUri = CommonHelper.GetAppValue("redirectUrl");
                    string URL = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + MyAppid + "&redirect_uri=" + RedirectUri + "&response_type=code&scope=snsapi_userinfo&state=a1#wechat_redirect";
                    string Str = "<a href='" + URL + "'>授权页面</a>";
                    res = sendTextMessage(wx, Str);
                }
                else if (wx.MsgType == "text" && wx.Content == "图文")
                {
                    res = sendPicTextMessage(wx, "这里是一个标题", "这里是摘要", "http://img.178.com/wow/201703/284669205048/284669235126.jpg", "http://www.163.com");
                }
                else if (wx.MsgType == "voice")//识别消息类型为语音  
                {
                    res = sendTextMessage(wx, wx.Recognition);//wx.Recognition就是语音识别的结果，我们直接引用，以文本形式反馈就OK了  
                }
                else if (wx.MsgType == "location")
                {
                    res = sendTextMessage(wx, "您发送的位置是:" + wx.Label + ";纬度是:" + wx.Location_X + ";经度是:" + wx.Location_Y + ";缩放比例为:" + wx.Scale);
                }               
                else
                {
                    res = sendTextMessage(wx, "你好,未能识别消息!");
                }
            }

            HttpContext.Current.Response.Write(res);
        }

        private Wxmessage GetWxMessage(Stream inputStream)
        {
            Wxmessage wx = new Wxmessage();
            StreamReader str = new StreamReader(inputStream, System.Text.Encoding.UTF8);
            XmlDocument xml = new XmlDocument();
            xml.Load(str);
            wx.ToUserName = xml.SelectSingleNode("xml").SelectSingleNode("ToUserName").InnerText;
            wx.FromUserName = xml.SelectSingleNode("xml").SelectSingleNode("FromUserName").InnerText;
            wx.MsgType = xml.SelectSingleNode("xml").SelectSingleNode("MsgType").InnerText;
            if (wx.MsgType.Trim() == "text")
            {
                wx.Content = xml.SelectSingleNode("xml").SelectSingleNode("Content").InnerText;
            }
            if (wx.MsgType.Trim() == "location")//回复发送的位置信息
            {
                wx.Location_X = xml.SelectSingleNode("xml").SelectSingleNode("Location_X").InnerText;
                wx.Location_Y = xml.SelectSingleNode("xml").SelectSingleNode("Location_Y").InnerText;
                wx.Scale = xml.SelectSingleNode("xml").SelectSingleNode("Scale").InnerText;
                wx.Label = xml.SelectSingleNode("xml").SelectSingleNode("Label").InnerText;
            }
            if (wx.MsgType.Trim() == "event")
            {
                wx.EventName = xml.SelectSingleNode("xml").SelectSingleNode("Event").InnerText;
                if (wx.EventName.ToUpper() == "LOCATION")//获取用户地理位置
                {
                    wx.Latitude = xml.SelectSingleNode("xml").SelectSingleNode("Latitude").InnerText;
                    wx.Longitude = xml.SelectSingleNode("xml").SelectSingleNode("Longitude").InnerText;
                    wx.Precision = xml.SelectSingleNode("xml").SelectSingleNode("Precision").InnerText;
                }
                else
                {
                    wx.EventKey = xml.SelectSingleNode("xml").SelectSingleNode("EventKey").InnerText;
                }
            }
            if (wx.MsgType.Trim() == "voice")//如果是语音消息的话就把识别结果赋值给实体类的相应属性Recognition   
            {
                wx.Recognition = xml.SelectSingleNode("xml").SelectSingleNode("Recognition").InnerText;
            }
            else if (wx.MsgType.Trim() == "image")
            {
                wx.MediaId = xml.SelectSingleNode("xml").SelectSingleNode("MediaId").InnerText;
            }  

            return wx;
        }

        /// <summary>    
        /// 发送文字消息    
        /// </summary>    
        /// <param name="wx">获取的收发者信息</param>    
        /// <param name="content">内容</param>
        /// <returns></returns>    
        private string sendTextMessage(Wxmessage wx, string content)
        {
            string res = string.Format(@"<xml>
                                   <ToUserName><![CDATA[{0}]]></ToUserName>  
                                   <FromUserName><![CDATA[{1}]]></FromUserName>  
                                    <CreateTime>{2}</CreateTime>  
                                    <MsgType><![CDATA[text]]></MsgType>  
                                    <Content><![CDATA[{3}]]></Content>
                                   </xml> ",
                wx.FromUserName, wx.ToUserName, DateTime.Now, content);
            return res;
        }

        /// <summary>
        /// 发送图文消息
        /// </summary>      
        private string sendPicTextMessage(Wxmessage wx, string title, string description, string picurl, string url)
        {
            string res = string.Format(@"<xml>  
                                            <ToUserName><![CDATA[{0}]]></ToUserName>  
                                            <FromUserName><![CDATA[{1}]]></FromUserName>  
                                            <CreateTime>{2}</CreateTime>  
                                            <MsgType><![CDATA[news]]></MsgType>  
                                            <ArticleCount>1</ArticleCount>  
                                            <Articles>  
                                            <item>  
                                            <Title><![CDATA[{3}]]></Title>   
                                            <Description><![CDATA[{4}]]></Description>  
                                            <PicUrl><![CDATA[{5}]]></PicUrl>  
                                            <Url><![CDATA[{6}]]></Url>  
                                            </item>
                                            </Articles></xml> ",
            wx.FromUserName, wx.ToUserName, DateTime.Now, title, description, picurl, url);

            return res;
        }


        /// <summary>
        /// 发送图片    
        /// </summary>
        /// <param name="_mode"></param>
        /// <param name="MediaId"></param>
        /// <returns></returns>
        private string sendPicTextMessage(Wxmessage wx, string MediaId)
        {
            string res = string.Format(@"<xml>  
                                            <ToUserName><![CDATA[{0}]]></ToUserName>  
                                            <FromUserName><![CDATA[{1}]]></FromUserName>  
                                            <CreateTime>{2}</CreateTime>  
                                            <MsgType><![CDATA[image]]></MsgType>  
                                            <Image>  
                                            <MediaId><![CDATA[{3}]]></MediaId>  
                                            </Image>  
                                   </xml> ",
               wx.FromUserName, wx.ToUserName, DateTime.Now, MediaId);

            return res;
        }  
       


    }
}