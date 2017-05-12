using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebChat.Model
{
    [Serializable]
    public class UserInfo
    {
        public int Id { get; set; }
        /// <summary>
        /// 用户唯一凭证
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 用户唯一凭证密钥
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// 全局唯一票据
        /// </summary>
        public string Access_token { get; set; }
        /// <summary>
        /// 加解密密钥
        /// </summary>
        public string EncodingAESKey { get; set; }
        /// <summary>
        /// 网页授权access_token
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }
    }
}
