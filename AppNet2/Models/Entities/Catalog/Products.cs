using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebAppNet2.Models.Entities.Carts;
using WebAppNet2.Models.Entities.Orders;

namespace WebAppNet2.Models.Entities.Catalog
{
    public class Products
    {
        [Key]
        public Guid ProductID { get; set; }

        public string ProductName { get; set; }

        public string? ProductDescription { get; set; }

        public double Price { get; set; }

        public string ImageProduct { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        // Foreign Key  

        public Guid CategoryID { get; set; }

        // Navigation Property


        [JsonIgnore]
        public Categories Category { get; set; }

        public ICollection<ColorSizes> ColorSizes { get; set; }

        [JsonIgnore]
        public ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();

        [JsonIgnore]
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
