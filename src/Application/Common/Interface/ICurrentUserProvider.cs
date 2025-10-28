namespace Application.Common.Interface;

public interface ICurrentUserProvider
{
    Guid? GetCurrentUserId();
}
