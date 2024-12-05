namespace WebAppNet2.Models.DTO
{
    public class CartItemDTO
    {
        public Guid ProductID { get; set; }
        public Guid ColorSizeID { get; set; }
        public Guid? ColorID { get; set; }

        public Guid? SizeID { get; set; }
        public int Quantity { get; set; }

    }
}
