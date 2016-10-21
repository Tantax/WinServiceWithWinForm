using IAICN.GasFee.DTO.SendSMS;
using IAICN.GasFee.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IAICN.GasFee.SendSMS.Service
{
    public static class TokenHelper
    {
        private static string _token;
        private static DateTime _expired;

        public static string Token
        {
            get
            {
                if (_expired.Equals(DateTime.MinValue) || _expired.AddHours(-1) < DateTime.Now)// 已过期
                {
                    var dto = GetToken();
                    if (dto.errcode == 0)
                    {
                        _token = dto.access_token;
                        _expired = dto.expired_at;
                    }
                    else// 获取token失败
                    {
                        // 写入日志
                        LogHelper.WriteLog(dto.errmsg);
                    }
                }
                return _token;
            }
        }
        public static DateTime Expired
        {
            get
            {
                return _expired;
            }
        }
        public static string MsgCenterUrl
        {
            get
            {
                return ConfigurationHelper.GetAppSettings("MsgCenterUrl");;
            }
        }

        public static TokenResponseDTO GetToken()
        {
            string source = ConfigurationHelper.GetAppSettings("Source");
            string user = ConfigurationHelper.GetAppSettings("User");
            string appSecret = ConfigurationHelper.GetAppSettings("AppSecret");
            string tokenApi = string.Format(ConfigurationHelper.GetAppSettings("TokenApi"), new object[] { source, user, appSecret });
            string tokenApiUrl = MsgCenterUrl + tokenApi;
            string result = HttpHelper.HttpGet(tokenApiUrl);
            return JsonConvert.DeserializeObject<TokenResponseDTO>(result);
        }        
    }
}
