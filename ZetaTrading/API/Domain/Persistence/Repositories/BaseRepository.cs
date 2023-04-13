using ZetaTrading.API.Domain.Persistence.Contexts;

namespace ZetaTrading.API.Domain.Persistence.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly AppDbContext _context;

        protected BaseRepository(AppDbContext context)
        {
            _context = context;
        }
    }
}
