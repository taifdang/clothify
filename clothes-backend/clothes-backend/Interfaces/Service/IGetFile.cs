namespace clothes_backend.Interfaces.Service
{
    public interface IGetFile
    {
        string getFileName(string file_name, string? attribute = null);
        string GetFilePath(string file_path);
    }
}
