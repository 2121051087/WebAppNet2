using WebAppNet2.Models.Entities.Orders;

namespace WebAppNet2.Models.DTO
{
    public class OrdersDTO
    {
        public string? fullName { get;set; } 
        public double? TotalAmount { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? District { get; set; }

        public string? Ward { get; set; }

     
        public string? PhoneNumber { get; set; }

        public List<CartItemDTO>?  cartItemDTOs { get; set;}

        //  attribute  for display

        
        public DateTime? OrderDate { get; set; }

        public string? Status { get; set; }

        public Guid? OrderID { get; set; }


        public List<OrderDetailDTO>? orderDetailDTO { get; set; }
    }


    public class OrderDetailDTO
    {
        public Guid OrderID { get; set; }

        public Guid? ProductID { get; set; }

        public string? ProductName { get; set; }

        public string? Image {  get; set; }

        public Guid ?ColorSizeID { get; set; }

        public double? Price { get; set; }

        public string ? SizeName { get; set; }

        public string? ColorName { get; set; }
        public int Quantity { get; set; }


    }
}
