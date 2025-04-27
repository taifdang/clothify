using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace clothes_backend.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration configuration)
        {
            _config = configuration;
        }
        public async Task EmailSender(string email, string otp)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("BOTCHAT", _config["MailKit:email"]));
            message.To.Add(new MailboxAddress("Mr. Client",email));
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
                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }
    }
}
