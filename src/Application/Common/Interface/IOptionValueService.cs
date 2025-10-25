namespace Application.Common.Interface;

public interface IOptionValueService
{
    Task<int> GetList();
    Task<int> Add();
    Task<int> Update();
    Task<int> Delete();

}
