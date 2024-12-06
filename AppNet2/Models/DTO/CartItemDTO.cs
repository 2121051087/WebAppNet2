namespace WebAppNet2.Models.DTO
{
    public class CartItemDTO
    {
        public Guid ProductID { get; set; }
        public Guid ColorSizeID { get; set; }
        public Guid? ColorID { get; set; }

        public Guid? SizeID { get; set; }
        public int Quantity { get; set; }


        // thuoc tinh hien thi tren gio hang 

        public Guid? Cart_itemID { get; set; }
        public string? ProductName { get; set; }

        public string? SizeName { get; set; }

        public string? ColorName { get; set; }

        public string? Image { get; set; }

        public double? Price { get; set; }

    }
}
