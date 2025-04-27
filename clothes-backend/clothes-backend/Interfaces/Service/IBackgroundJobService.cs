namespace clothes_backend.Interfaces.Service
{
    public interface IBackgroundJobService
    {
        Task FireAndForgetJob(List<string> file);
        void RecurringJob();
        Task DelayedJob(string email,string otp);
        void ContinuationJob();

    }
}
