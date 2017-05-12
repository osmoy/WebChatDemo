using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebChar.Model
{
    [Serializable]
    public class Ticket
    {
        public Ticket()
        {
            //  
            //TODO: 在此处添加构造函数逻辑  
            //  
        }

        string _ticket;
        string _expire_seconds;

        /// <summary>  
        /// 获取的二维码ticket，凭借此ticket可以在有效时间内换取二维码。  
        /// </summary>  
        public string ticket
        {
            get { return _ticket; }
            set { _ticket = value; }
        }

        /// <summary>  
        /// 凭证有效时间，单位：秒  
        /// </summary>  
        public string expire_seconds
        {
            get { return _expire_seconds; }
            set { _expire_seconds = value; }
        }  
    }
}
