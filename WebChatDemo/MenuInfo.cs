using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Net;
using WebChat.Common;

namespace WebChatDemo
{
    public class MenuInfo
    {
        private static string MyMenu
        {
            get
            {
                return @" {  
                             ""button"":[  
                             {    
                                ""name"": ""扫码"", 
                                ""sub_button"": [
                                             {
                                                ""type"": ""scancode_waitmsg"", 
                                                ""name"": ""扫码带提示"", 
                                                ""key"": ""rselfmenu_0_0"", 
                                                ""sub_button"": [ ]
                                            }, 
                                            {
                                                ""type"": ""scancode_push"", 
                                                ""name"": ""扫码推事件"", 
                                                ""key"": ""rselfmenu_0_1"", 
                                                ""sub_button"": [ ]
                                            }
                                                ]                                  
                              },  
                              {  
                                  ""name"": ""发图"", 
                                  ""sub_button"": [
                                             {
                                                ""type"": ""pic_sysphoto"", 
                                                ""name"": ""系统拍照发图"", 
                                                ""key"": ""rselfmenu_1_0"", 
                                                ""sub_button"": [ ]
                                            }, 
                                            {
                                                ""type"": ""pic_weixin"", 
                                                ""name"": ""微信相册发图"", 
                                                ""key"": ""rselfmenu_1_2"", 
                                                ""sub_button"": [ ]
                                            },
                                            {
                                                ""type"":""click"",
                                                ""name"":""点个赞呗"",
                                                ""key"":""V1001_GOOD""
                                            }
                                                ]
                              },  
                              {  
                                   ""name"":""设置"",
                                   ""sub_button"":[
                                            {  
                                               ""name"":""发送位置"", 
                                               ""type"":""location_select"",
                                               ""key"":""rselfmenu_2_0""  
                                            },  
                                            {  
                                               ""type"":""view"",  
                                               ""name"":""我的信息"",
                                               ""url"":""http://121.41.53.108/ashx/RediRectUrl.ashx""
                                            },
                                            {
                                                ""type"":""click"",
                                                ""name"":""今日头条"",
                                                ""key"":""today""
                                            }
                                    ]
                               }]
                         }";
            }
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        public static void CreateMenuPage()
        {
            var posturl = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", PartenerInfo.IsTokenExpired());
            var content = PartenerInfo.GetPage(posturl, MyMenu);
            HttpContext.Current.Response.Write(content);
        }

        /// <summary>
        /// 删除菜单
        /// </summary>        
        public static void DelMenu()
        {
            string postUrl = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}";
            postUrl = string.Format(postUrl, PartenerInfo.IsTokenExpired());
            var strJson = PartenerInfo.RequestUrl(postUrl, "GET");
            var errmsg = CommonHelper.GetJsonValue(strJson, "errmsg");
            HttpContext.Current.Response.Write(errmsg);
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        public static string GetMenuList(string posturl)
        {
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                HttpContext.Current.Response.Write(content);
                return content;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return string.Empty;
            }
        }




    }
}