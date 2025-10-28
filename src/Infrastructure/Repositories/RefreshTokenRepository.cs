using Infrastructure.Data;
using Infrastructure.Enitites;
using Infrastructure.Interface;

namespace Infrastructure.Repositories;

public class RefreshTokenRepository(ApplicationDbContext context) : BaseRepository<RefreshToken>(context), IRefreshTokenRepository
{
  
}
