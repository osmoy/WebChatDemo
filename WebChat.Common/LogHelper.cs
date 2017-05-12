using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace WebChat.Common
{
    public class LogHelper
    {
        /// <summary>  
        /// 写日志(用于跟踪)  
        /// </summary>  
        public static void WriteLog(string strMemo)
        {
            var context = HttpContext.Current;
            string filename = context.Server.MapPath("/logs/log.txt");
            if (!Directory.Exists(context.Server.MapPath("//logs//")))
                Directory.CreateDirectory("//logs//");
            StreamWriter sr = null;
            try
            {
                if (!File.Exists(filename))
                {
                    sr = File.CreateText(filename);
                }
                else
                {
                    sr = File.AppendText(filename);
                }
                sr.WriteLine(strMemo);
            }
            catch
            {

            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
        }  
    }
}
