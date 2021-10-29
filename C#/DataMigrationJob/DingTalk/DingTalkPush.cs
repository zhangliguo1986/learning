using EagleEye.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Eagle.Exam8.Utils.DingTalk
{
    /// <summary>
    /// 钉钉推送服务类
    /// </summary>
    public class DingTalkPush
    {

        #region 推送到钉钉（发送/签名）等方法

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="param"></param>
        /// <param name="Webhook"></param>
        /// <param name="dingTalksecret"></param>
        public static void PushMessage(DingTalkMsgEntity msgEntity, string Webhook, string dingTalksecret)
        {
            var msg = JsonConvert.SerializeObject(msgEntity);
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long timestamp = (long)ts.TotalMilliseconds;
            string posturl = string.Format(Webhook + "&timestamp={0}&sign={1}", timestamp, Sign(timestamp, dingTalksecret));
            HttpHelper httpHelper = new HttpHelper();
            string result = httpHelper.HttpInterAction(posturl, msg, HttpMethod.POST, contentType: "application/json;charset=UTF-8");
        }

        /// <summary>
        /// 构建签名
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="dingTalksecret"></param>
        /// <returns></returns>
        private static string Sign(long timestamp, string dingTalksecret)
        {
            String stringToSign = timestamp + "\n" + dingTalksecret;
            byte[] signData = HmacSHA256(Encoding.UTF8.GetBytes(dingTalksecret), Encoding.UTF8.GetBytes(stringToSign));
            return HttpUtility.UrlEncode(Convert.ToBase64String(signData), Encoding.UTF8);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="key"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private static byte[] HmacSHA256(byte[] key, byte[] content)
        {
            using (var hmacsha256 = new HMACSHA256(key))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(content);
                return hashmessage;
            }
        }
        #endregion

    }
}
