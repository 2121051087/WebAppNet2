using WebAppNet2.Infrastructures.Repositories;

namespace WebAppNet2.Infrastructures.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICategoriesRepository CategoriesRepository { get; set; }
        IProductRepository ProductRepository { get; set; }
        ICartRepository CartRepository { get; set; }

        IOrdersRepository OrdersRepository { get; set; }
      


        Task SaveChangeAsync();
    }
}
