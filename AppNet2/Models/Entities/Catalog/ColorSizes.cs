using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebAppNet2.Models.Entities.Carts;
using WebAppNet2.Models.Entities.Orders;

namespace WebAppNet2.Models.Entities.Catalog
{
    public class ColorSizes
    {
        [Key]
        public Guid ColorSizesID { get; set; }

        public int Quantity { get; set; }

        // Foreign Key
        public Guid ColorID { get; set; }

        public Guid SizeID { get; set; }

        public Guid ProductID { get; set; }

        // Navigation Property

        [JsonIgnore]
        public Products Product { get; set; }
        [JsonIgnore]
        public Sizes sizes { get; set; }
        [JsonIgnore]
        public Colors Color { get; set; }

        [JsonIgnore]
        public ICollection<CartItem> cartItems { get; set; }
        [JsonIgnore]
        public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
