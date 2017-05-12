using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebChat.Model
{
    [Serializable]
    public class Wxmessage
    {
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public string MsgType { get; set; }
        public string EventName { get; set; }
        public string Content { get; set; }
        public string EventKey { get; set; }
        public string Recognition { get; set; }
        public string Location_X { get; set; }
        public string Location_Y { get; set; }
        public string Scale { get; set; }
        public string Label { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Precision { get; set; }
        /// <summary>
        /// 微信服务器上的资源id
        /// </summary>
        public string MediaId { get; set; }  
    }
}
