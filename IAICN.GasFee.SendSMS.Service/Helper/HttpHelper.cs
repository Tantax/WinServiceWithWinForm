using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IAICN.GasFee.Infrastructure;

namespace IAICN.GasFee.SendSMS.Service
{
    public static class HttpHelper
    {
        public static string HttpPost(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/json";
            //request.CookieContainer = cookies;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("utf-8"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();            
            //response.Cookies = cookies.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
        public static string HttpGet(string Url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        public static string GetInternetIP()
        {
            try
            {
                string strUrl = ConfigurationHelper.GetAppSettings("IP");     //获得IP的网址
                string result = HttpGet(strUrl);//读取网站返回的数据格式：您的IP地址是：[x.x.x.x]       
                int i = result.IndexOf("[") + 1;
                string tempip = result.Substring(i, 15);
                string ip = tempip.Replace("]", "").Replace(" ", "").Replace("<", "");     //去除杂项找出ip
                return ip;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IP获取失败，错误信息：" + ex.Message, ex);
                return string.Empty;
            }
        }
    }
}
