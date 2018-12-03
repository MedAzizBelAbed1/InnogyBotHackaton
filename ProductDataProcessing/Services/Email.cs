using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace ProductDataProcessing.Services
{
	public class Email
	{
		public static string SendMail( string userName ,string data, string email)
		{
			string result = string.Empty;
			// get all  infroamtions as json Format
			MailMessage msg = new MailMessage();
			// need to inser an email here 
			msg.From = new MailAddress("");
			msg.To.Add(email);
			msg.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
			//msg.IsBodyHtml = true;
			msg.Body = data;
			msg.Priority = MailPriority.Normal;
			msg.Subject = $"{userName}";
			SmtpClient smtp = new SmtpClient();
			smtp.Host = "smtp.office365.com";
			smtp.EnableSsl = true;
			smtp.Port = 25;
			//FIXME need to insert email details 
			smtp.Credentials = new System.Net.NetworkCredential("", "");
			try
			{
				smtp.Send(msg);
				result = "Email Sent";
			}
			catch (Exception ex)
			{
				result = $"Sending Email Error : {ex.Message}";
			}
			return result;
		}

	}
}