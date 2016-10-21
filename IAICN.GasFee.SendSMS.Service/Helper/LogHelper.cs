using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAICN.GasFee.SendSMS.Service
{
    public class LogHelper
    {
  
        public static readonly ILog loginfo = LogManager.GetLogger("loginfo");   //选择<logger name="loginfo">的配置 
   
        public static readonly ILog logerror = LogManager.GetLogger("logerror");   //选择<logger name="logerror">的配置 
   
        public static void SetConfig()   
        {   
            XmlConfigurator.Configure();   
        }   
  
        public static void SetConfig(FileInfo configFile)   
        {   
            XmlConfigurator.Configure(configFile);    
        }   
  
        public static void WriteLog(string info)   
        {   
            if(loginfo.IsInfoEnabled)   
            {   
                loginfo.Info(info);   
            }   
        }   
  
        public static void WriteLog(string info,Exception se)   
        {   
            if(logerror.IsErrorEnabled)   
            {   
                logerror.Error(info,se);   
            }   
        } 
    }
}
