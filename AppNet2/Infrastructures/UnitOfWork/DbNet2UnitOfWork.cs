using AppNet2.Models;
using WebAppNet2.Infrastructures.Repositories;

namespace WebAppNet2.Infrastructures.UnitOfWork
{
    public class DbNet2UnitOfWork : IUnitOfWork
    {
        public ICategoriesRepository CategoriesRepository { get; set; }

        private readonly DbNet2Context _context;
        public DbNet2UnitOfWork(ICategoriesRepository categoriesRepository , DbNet2Context context)
        {
            CategoriesRepository = categoriesRepository;
            _context = context;
        }


        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
