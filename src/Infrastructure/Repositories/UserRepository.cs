using Infrastructure.Data;
using Infrastructure.Enitites;
using Infrastructure.Interface;

namespace Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context) : BaseRepository<User>(context), IUserRepository
{
}
