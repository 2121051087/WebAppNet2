using System.Text.Json.Serialization;
using WebAppNet2.Models.Entities.Catalog;

namespace WebAppNet2.Models.Entities.Orders
{
    public class OrderDetails
    {
        public Guid OrderDetailID { get; set; }

        public Guid OrderID { get; set; }

        public Guid ProductID { get; set; }

        public Guid ColorSizesID { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        // Navigation Property  
        [JsonIgnore]
        public Products Products { get; set; }

        public ColorSizes ColorSizes { get; set; }

        public Orders Orders { get; set; }
    }
}
