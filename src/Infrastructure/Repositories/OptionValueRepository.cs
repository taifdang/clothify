using Infrastructure.Data;
using Infrastructure.Enitites;
using Infrastructure.Interface;

namespace Infrastructure.Repositories;

public class OptionValueRepository(ApplicationDbContext context) : BaseRepository<OptionValue>(context), IOptionValueRepository
{
}
