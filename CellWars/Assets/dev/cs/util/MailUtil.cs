using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

public class MailUtil
{
    private static SmtpClient smtpClient = null;// 设置smtp协议
    private static MailMessage mailMessage_mai = null; //设置邮件信息,要发送的内容

    /// <summary>
    /// 发邮件
    /// </summary>
    /// <param name="smtp">邮箱服务器名称</param>
    /// <param name="affix">附件路径</param>
    /// <param name="from">发件箱地址</param>
    /// <param name="pwd">发件箱密码</param>
    /// <param name="to">收件箱地址</param>
    /// <param name="title">邮件标题</param>
    /// <param name="body">邮件正文</param>
    /// <returns></returns>
    public static bool Send(string smtp, string affix,
        string from, string pwd, string to, string title, string body)
    {
        smtpClient = new SmtpClient(smtp,25);
        smtpClient.UseDefaultCredentials = false;

        //指定服务器认证
        //NetworkCredential network = new NetworkCredential(from, pwd);
        //指定发件人信息,包括邮箱地址和密码
        NetworkCredential nc = new NetworkCredential(from, pwd);
        smtpClient.Credentials = (ICredentialsByHost)(nc); //这个在手机平台不成功
        //指定如何发送邮件
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

        //创建mailMessage对象
        mailMessage_mai = new MailMessage(from, to);
        mailMessage_mai.Subject = title;

        //设置正文默认格式为html
        mailMessage_mai.Body = body;
        mailMessage_mai.IsBodyHtml = true;
        mailMessage_mai.BodyEncoding = Encoding.UTF8;

        //添加附件
        if (!string.IsNullOrEmpty(affix))
        {
            Attachment data = new Attachment(affix, MediaTypeNames.Application.Octet);
            mailMessage_mai.Attachments.Add(data);
        }

        try
        {
            //发送
            smtpClient.SendAsync(mailMessage_mai, "000000000");
            Debug.Log("Send suc");
            return true;//返回true则发送成功
        }
        catch (Exception)
        {
            Debug.Log("Send err");
            return false;//返回false则发送失败
        }
    }

    public static void SendErrorLog(string strContent)
    {
#if !UNITY_EDITOR
        Send("qq.mail.com", string.Empty, "939906498@qq.com", "nevergiveupcyzy", "939906498@qq.com", "ErrorLog", strContent);
#endif
    }

    public enum GameLogType
    {
        other = 0,              //其他
        login = 1,              //登录
        level_result = 2,       //闯关结果
        login_GC = 3,           //登录gamecenter
    }

    public static void SendGameLog(GameLogType logtype,string strContent)
    {
#if !UNITY_EDITOR
        Send("smtp.163.com", string.Empty, "gemagame@163.com", "pop120660", "log_gemagame@163.com", "GameLog:" + (int)logtype, strContent);
#endif
    }

}