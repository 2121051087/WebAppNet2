using AppNet2.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAppNet2.Models.Entities.Carts;

namespace WebAppNet2.Infrastructures.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly DbNet2Context _context;
        private readonly ClaimsPrincipal? _user;

        public CartRepository(DbNet2Context context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _user = httpContextAccessor.HttpContext?.User;
        }
        public async Task<Carts> GetOrCreateCartAsync(string userID)
        {
            var existingCart = await _context.Carts
                                            .Include(c => c.cartItems)
                                            .ThenInclude(ci => ci.Products)
                                            .FirstOrDefaultAsync(c => c.UserID == userID);

            if(existingCart != null)
            {
                return existingCart;
            }

            var newCart = new Carts
            {
                UserID = userID,
                CreatedAt = DateTime.UtcNow.AddHours(7),
                UpdatedAt = DateTime.UtcNow.AddHours(7)
            };
            
            await _context.Carts.AddAsync(newCart);
            await _context.SaveChangesAsync();

            return newCart;

                                           
        }
    }
}
