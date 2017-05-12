using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebChat.Model
{
    [Serializable]
    public class AccessToken
    {
        /// <summary>  
        /// 获取到的凭证   
        /// </summary>  
        //public string access_token { get; set; }
        public string access_token = "";


        /// <summary>  
        /// 凭证有效时间，单位：秒  
        /// </summary>
        //public string expires_in { get; set; }
        public string expires_in = "";

    }
}
