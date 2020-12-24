using System;

namespace DelegateAndEvent
{
    /// <summary>
    /// 声明（定义）一个委托
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public delegate string SendDelegate(string message);

    public class HR
    {
        /// <summary>
        /// 委托实例
        /// </summary>
        public SendDelegate sendDelegate;

        public void SendMessage(string msg)
        {
            sendDelegate(msg);
        }


    }
}
