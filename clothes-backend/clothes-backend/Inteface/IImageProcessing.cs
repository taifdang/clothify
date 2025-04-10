namespace clothes_backend.Inteface
{
    public interface IImageProcessing
    {
        Task saveImage(IFormFile file, string file_path);
        string getFileName(string file_name,string attribute);
        string getFilePath(string file_path);
       
    }
}
