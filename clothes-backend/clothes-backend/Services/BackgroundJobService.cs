using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
using clothes_backend.Interfaces.Service;
using System.Threading.Tasks;

namespace clothes_backend.Services
{
    public class BackgroundJobService : IBackgroundJobService
    {
        private readonly EmailService _emailSender;
        private readonly RemoveFileService _fileService;
        public BackgroundJobService(EmailService emailService, RemoveFileService fileService) { _emailSender = emailService; _fileService = fileService; }
        public void ContinuationJob()
        {
            throw new NotImplementedException();
        }

        public void DelayedJob()
        {
            throw new NotImplementedException();
        }

        public virtual async Task DelayedJob(string email, string otp)
        {
            await _emailSender.EmailSender(email,otp);
        }

        public async Task FileWorker(IFormFile[] file)
        {
            foreach (var item in file)
            {
                var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", item.FileName);
                using (var stream = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                   await  item.CopyToAsync(stream);
                }
            }
        }

        public async Task FireAndForgetJob(List<string> file)
        {
            await _fileService.RemoveFile(file);
        }     
        public void RecurringJob()
        {
            throw new NotImplementedException();
        }
    }
}
