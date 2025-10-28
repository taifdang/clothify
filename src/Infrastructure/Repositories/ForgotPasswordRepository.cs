using Infrastructure.Data;
using Infrastructure.Enitites;
using Infrastructure.Interface;

namespace Infrastructure.Repositories;

public class ForgotPasswordRepository(ApplicationDbContext context) : BaseRepository<ForgotPassword>(context), IForgotPasswordRepository
{
}
