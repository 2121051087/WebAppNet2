using WebAppNet2.Models.DTO;
using WebAppNet2.Models.Entities.Carts;

namespace WebAppNet2.Infrastructures.Repositories
{
    public interface ICartRepository
    {
        public Task<Carts> GetOrCreateCartAsync(string userID);
        public Task AddItemToCartAsync(CartItemDTO model);

        
    }
}
