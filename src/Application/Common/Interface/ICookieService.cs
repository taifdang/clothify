namespace Application.Common.Interface;

public interface ICookieService
{
    public void Set(string token);
    public string Get();
    public void Delete();
}
