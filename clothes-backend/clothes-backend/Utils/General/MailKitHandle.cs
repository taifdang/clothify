using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
namespace clothes_backend.Utils.General
{
    public class MailKitHandle
    {
        private readonly IConfiguration _config;
        public MailKitHandle(IConfiguration config) { _config = config; }
        public  bool sendMail(string email,string userName) {
            try
            {

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("BOTCHAT", _config["MailKit:email"]));
                message.To.Add(new MailboxAddress("Mr. Client", email));
                message.Subject = "Test smtp gmail";
                //message.Body = new TextPart("plain")
                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text =$@"<p>Mã OTP của bạn là: <strong>{Random.Shared.Next(1000, 9999).ToString()}</strong><br>Mã có hiệu lực trong 5 phút: {DateTime.Now.ToString("yyyy/mm/dd hh:mm:ss tt")}<br> Vui lòng xác thực để hoàn tất đăng ký</p>"                   
                };
                using (var client = new SmtpClient())    {
                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    client.Authenticate(_config["MailKit:email"], _config["MailKit:pass"]);
                    client.Send(message);
                    client.Disconnect(true);
                    return true;
                };               
            }
            catch
            {
                return false;
            }
        }
    }
}
