namespace clothes_backend.Services
{
    public class RemoveFileService
    {
        public async Task RemoveFile(List<string> files)
        {
            if (files != null)
            {
                var full_path = Path.Combine(Directory.GetCurrentDirectory(), "Images");
                foreach (var file in files)
                {
                    var path = Path.Combine(full_path, file);
                    if (File.Exists(path))
                    {
                        await Task.Run(() => File.Delete(path));
                    }
                    else continue;
                }
            }
        }
    }
}
