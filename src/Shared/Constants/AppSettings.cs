using System.ComponentModel.DataAnnotations;

namespace Shared.Constants;

public class AppSettings
{
    public Identity Identity { get; set; }
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
public class Identity
{
    [Required]
    public bool IsLocal { get; set; } = false;
    [Required]
    public string Key { get; set; }
    [Required]
    public string Issuer { get; set; }
    [Required]
    public string Audience { get; set; }
    [Required]
    public string ScopeBaseDomain { get; set; }
    [Required]
    public bool ValidateHttps { get; set; }
    public int ExpiredTime { get; set; } = 10;
}
