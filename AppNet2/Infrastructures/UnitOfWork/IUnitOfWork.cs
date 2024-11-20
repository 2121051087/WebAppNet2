using WebAppNet2.Infrastructures.Repositories;

namespace WebAppNet2.Infrastructures.UnitOfWork
{
    public interface IUnitOfWork
    {
       ICategoriesRepository CategoriesRepository { get; set; }

        Task SaveChangeAsync();
    }
}
