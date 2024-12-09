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
    }



}
