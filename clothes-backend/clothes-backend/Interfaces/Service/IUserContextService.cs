namespace clothes_backend.Interfaces.Service
{
    public interface IUserContextService
    {
        string getValueAuth();
        int convertToInt(string input,int default_value = 0);
    }
}
