using System.Text.Json.Serialization;
using WebAppNet2.Models.Entities.Catalog;

namespace WebAppNet2.Models.Entities.Carts
{
    public class CartItem
    {

        public Guid Cart_itemID { get; set; }

        public Guid CartID { get; set; }

        public Guid ProductID { get; set; }

        public Guid ColorSizesID { get; set; }

        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        // Navigation Property 

        public Products Products { get; set; }

        public ColorSizes ColorSizes { get; set; }

        [JsonIgnore]
        public Carts Carts { get; set; }
    }
}
