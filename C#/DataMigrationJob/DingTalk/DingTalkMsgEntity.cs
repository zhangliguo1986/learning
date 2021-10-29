using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Exam8.Utils.DingTalk
{
    /// <summary>
    /// DingTalk 推送消息实体
    /// </summary>
    public class DingTalkMsgEntity
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public MsgTypeEnum msgtype { get; set; }

        /// <summary>
        /// 文本 消息实体
        /// </summary>
        public DingTalkMsgForText text { get; set; }

        /// <summary>
        /// Markdown 消息实体
        /// </summary>
        public DingTalkMsgForMarkdown markdown { get; set; }
    }

    /// <summary>
    /// 文本 消息实体
    /// </summary>
    public class DingTalkMsgForText
    {
        public string context { get; set; }
    }

    /// <summary>
    /// markdown 消息实体
    /// </summary>
    public class DingTalkMsgForMarkdown
    {
        public string title { get; set; }

        public string text { get; set; }

        public string[] atMobiles { get; set; }

        public string[] atUserIds { get; set; }

        public bool isAtAll { get; set; }
    }

    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MsgTypeEnum
    {
        text,
        link,
        markdown,
        actionCard,
        feedCard
    }
}
