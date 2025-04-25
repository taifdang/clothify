namespace clothes_backend.Inteface.Utils
{
    public interface IBackgroundJobService
    {
        void FireAndForgetJob();
        void RecurringJob();
        void DelayedJob(string email,string otp);
        void ContinuationJob();

    }
}
