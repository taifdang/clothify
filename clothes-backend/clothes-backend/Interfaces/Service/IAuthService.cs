namespace clothes_backend.Interfaces.Service
{
    public interface IAuthService
    {
        string getValueAuth();
        int convertToInt(string input,int default_value = 0);
    }
}
