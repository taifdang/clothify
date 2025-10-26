using System.ComponentModel.DataAnnotations;

namespace Shared.Constants;

public class AppSettings
{
    public FileStorageSettings FileStorageSettings { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
    public string BaseUrl { get; set; } = "";
}
public class ConnectionStrings
{
    [Required]
    public string DefaultConnection { get; set; }
}
public class FileStorageSettings
{
    public bool LocalStorage { get; set; } = true;
    [Required]
    public string Path { get; set; } = "";
}