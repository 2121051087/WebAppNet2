using Microsoft.AspNetCore.Identity;
using WebAppNet2.Models.Entities.Carts;

namespace AppNet2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;


        public ICollection<Carts> Carts { get; set; } = new List<Carts>();

        
    }
}
