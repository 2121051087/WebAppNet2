using WebAppNet2.Models.DTO;

namespace WebAppNet2.Infrastructures.Repositories
{
    public interface IOrdersRepository
    {
        public Task CreateOrderAsync(OrdersDTO model);
    }
}
