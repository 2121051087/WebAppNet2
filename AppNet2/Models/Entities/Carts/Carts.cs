using System.ComponentModel.DataAnnotations;

namespace WebAppNet2.Models.Entities.Carts
{
    public class Carts
    {
        [Key]
        public Guid CartID { get; set; }

        public Guid UserID { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        // Navigation Property


        public ICollection<CartItem> cartItems { get; set; } = new List<CartItem>();
    }

}
