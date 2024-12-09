using AppNet2.Models;
using WebAppNet2.Infrastructures.Repositories;

namespace WebAppNet2.Infrastructures.UnitOfWork
{
    public class DbNet2UnitOfWork : IUnitOfWork
    {
        public ICategoriesRepository CategoriesRepository { get; set; }
        public IProductRepository ProductRepository { get; set ; }
        public ICartRepository CartRepository { get ; set; }
        public IOrdersRepository OrdersRepository { get ; set ; }


        private readonly DbNet2Context _context;
        public DbNet2UnitOfWork(ICategoriesRepository categoriesRepository , DbNet2Context context, IProductRepository productRepository ,ICartRepository cartRepository,IOrdersRepository ordersRepository)
        { 
            CategoriesRepository = categoriesRepository;
            _context = context;
            ProductRepository = productRepository;
            CartRepository = cartRepository;
            OrdersRepository = ordersRepository;
       
        }


        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
