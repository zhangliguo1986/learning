using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace Eagle.Exam8.Utils
{
    public class MailHelper
    {
        public static void SendAsyncEmail(string strSmtpServer, string strFrom, string strFromPass, string strto, string strSubject, string strBody, bool isHtmlFormat, string[] files, object userToken, SendCompletedEventHandler onComplete)
        {
            try
            {
                SmtpClient client = new SmtpClient(strSmtpServer);
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(strFrom, strFromPass);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage message = new MailMessage(strFrom, strto, strSubject, strBody);
                message.BodyEncoding = Encoding.Default;
                message.IsBodyHtml = isHtmlFormat;
                if (files != null)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (File.Exists(files[i]))
                        {
                            message.Attachments.Add(new Attachment(files[i]));
                        }
                    }
                }
                client.SendCompleted += new SendCompletedEventHandler(onComplete.Invoke);
                client.SendAsync(message, userToken);
            }
            catch (Exception exception)
            {
                throw new Exception("发送邮件失败。错误信息：" + exception.Message);
            }
        }

        public static void SendEmail(string strSmtpServer, string strFrom, string strFromPass, string strto, string strSubject, string strBody, bool isHtmlFormat)
        {
            SendEmail(strSmtpServer, strFrom, strFromPass, strto, strSubject, strBody, isHtmlFormat, null);
        }

        public static void SendEmail(string strSmtpServer, string strFrom, string strFromPass, string strto, string strSubject, string strBody, bool isHtmlFormat, string[] files)
        {
            //try
            //{
            //    SmtpClient client = new SmtpClient(strSmtpServer);
            //    client.UseDefaultCredentials = false;
            //    client.Credentials = new NetworkCredential(strFrom, strFromPass);
            //    client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //    MailMessage message = new MailMessage(strFrom, strto, strSubject, strBody);
            //    message.BodyEncoding = Encoding.Default;
            //    message.IsBodyHtml = isHtmlFormat;
            //    if (files != null)
            //    {
            //        for (int i = 0; i < files.Length; i++)
            //        {
            //            if (File.Exists(files[i]))
            //            {
            //                message.Attachments.Add(new Attachment(files[i]));
            //            }
            //        }
            //    }
            //    client.Send(message);
            //}
            //catch (Exception exception)
            //{
            //    throw new Exception("发送邮件失败。错误信息：" + exception.Message);
            //}

            //确定smtp服务器地址 实例化一个Smtp客户端
            SmtpClient smtpclient = new SmtpClient();
            smtpclient.Host = strSmtpServer;
            //smtpClient.Port = "";//qq邮箱可以不用端口

            //确定发件地址与收件地址
            MailAddress sendAddress = new MailAddress(strFrom);
            MailAddress receiveAddress = new MailAddress(strto);
            //构造一个Email的Message对象内容信息
            MailMessage mailMessage = new MailMessage(sendAddress, receiveAddress);
            mailMessage.Subject = strSubject;
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            mailMessage.Body = strBody;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.IsBodyHtml = isHtmlFormat;

            //邮件发送方式  通过网络发送到smtp服务器
            smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;
            //如果服务器支持安全连接，则将安全连接设为true
            smtpclient.EnableSsl = true;
            try
            {
                //是否使用默认凭据，若为false，则使用自定义的证书，就是下面的networkCredential实例对象
                smtpclient.UseDefaultCredentials = false;
                //指定邮箱账号和密码,需要注意的是，这个密码是你在QQ邮箱设置里开启服务的时候给你的那个授权码
                NetworkCredential networkCredential = new NetworkCredential(strFrom, strFromPass);
                smtpclient.Credentials = networkCredential;
                if (files != null)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (File.Exists(files[i]))
                        {
                            mailMessage.Attachments.Add(new Attachment(files[i]));
                        }
                    }
                }
                //发送邮件
                smtpclient.Send(mailMessage);
            }
            catch (System.Net.Mail.SmtpException exception)
            {
                //Console.WriteLine(exception.Message, "发送邮件出错");
                throw new Exception($"发送邮件失败，错误信息：{exception.Message}");
            }
            catch (Exception exception)
            {
                throw new Exception($"发送邮件失败，错误信息：{exception.Message}");
            }
        }
    }
}
