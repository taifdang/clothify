namespace clothes_backend.Inteface.User
{
    public interface IAuthService
    {
        string getValueAuth();
        int convertToInt(string input,int default_value = 0);
    }
}
