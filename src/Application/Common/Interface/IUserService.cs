using Shared.Models.User;

namespace Application.Common.Interface;

public interface IUserService
{
    public Task<List<UserReadModel>> GetAvailable(CancellationToken cancellationToken);
    public Task AssignRole(AssignRoleRequest request, CancellationToken cancellationToken);
    public Task Update(UserUpdateRequest request, CancellationToken cancellationToken);
    public Task Delete(string userId);
}
