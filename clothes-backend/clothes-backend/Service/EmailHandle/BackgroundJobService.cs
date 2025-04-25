using clothes_backend.Inteface.Utils;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
namespace clothes_backend.Service.EmailHandle
{
    public class BackgroundJobService : IBackgroundJobService
    {
        private readonly IConfiguration _config;
        public BackgroundJobService(IConfiguration config) { _config = config; }
        public void ContinuationJob()
        {
            throw new NotImplementedException();
        }

        public void DelayedJob()
        {
            throw new NotImplementedException();
        }

        public void DelayedJob(string email, string otp)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("BOTCHAT", _config["MailKit:email"]));
            message.To.Add(new MailboxAddress("Mr. Client", email));
            message.Subject = "Test smtp gmail";
            //message.Body = new TextPart("plain")
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $@"<p>Mã OTP của bạn là: <strong>{otp}</strong><br>Mã có hiệu lực trong 5 phút: {DateTime.Now.ToString("yyyy/mm/dd hh:mm:ss tt")}<br> Vui lòng xác thực để hoàn tất đăng ký</p>"
            };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                client.Authenticate(_config["MailKit:email"], _config["MailKit:pass"]);
                client.Send(message);
                client.Disconnect(true);
            }
        }
        public void FireAndForgetJob()
        {
            throw new NotImplementedException();
        }

        public void RecurringJob()
        {
            throw new NotImplementedException();
        }
    }
}
