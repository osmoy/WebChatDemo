using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace WebChat.Common
{
    public class JsonHelper
    {
        /// <summary>  
        /// 生成Json格式  
        /// </summary>  
        public static string GetJson<T>(T obj)
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                json.WriteObject(stream, obj);
                string szJson = Encoding.UTF8.GetString(stream.ToArray()); 
                return szJson;
            }
        }

        /// <summary>  
        /// 获取Json的Model  
        /// </summary>  
        public static T ParseFromJson<T>(string szJson)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(szJson)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                return (T)serializer.ReadObject(ms);
            }
        }

    }
}
