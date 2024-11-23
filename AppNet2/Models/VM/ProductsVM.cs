namespace WebAppNet2.Models.DTO
{
    public class ProductsVM
    {

        public string ProductName { get; set; }

        public string? ProductDescription { get; set; }

        public double Price { get; set; }
        public int CategoryID { get; set; }

        public string colorSizesDTO { get; set; }

    }

    public class ColorSidesDTO
    {

        public int ColorID { get; set; }
        public int SizeID { get; set; }
        public int Quantity { get; set; }
    }
}    

