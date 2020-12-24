using System;

namespace DelegateAndEvent
{
    public class DelegateTest
    {
        public static void Test()
        {
            HR hr = new HR();

            //猎头张三来监听，听到HR发消息后立刻传播出去
            Sender senderZS = new Sender("张三");
            hr.sendDelegate = senderZS.Send;

            Sender senderLS = new Sender("李四");
            hr.sendDelegate += senderLS.Send;

            //HR递交消息
            hr.SendMessage("Hello World");
        }

    }
}