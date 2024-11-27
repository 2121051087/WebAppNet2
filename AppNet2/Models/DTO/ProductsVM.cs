using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAppNet2.Models.DTO
{
    public class ProductsVM
    {
        public Guid ProductID { get; set; }

        [DisplayName("Tên sản phẩm")]
        public string ProductName { get; set; }

        [DisplayName("Mô tả")]
        public string? ProductDescription { get; set; }

        [DisplayName("Giá")]
        public double Price { get; set; }

        public IFormFile? ImagePath { get; set; }
        public Guid CategoryID { get; set; }

        [DisplayName("Ảnh")]
        public string? CurrentImagePath { get; set; }

        [DisplayName("Ngày lập")]
        public DateTime UpdatedAt { get; set; }
        [DisplayName("Danh mục")]
        public string? CategoryName { get; set; }

        [DisplayName("Thông số")]
        public List<ColorSidesDTO> colorSizesDTO { get; set; } = null!;



    }

    public class ColorSidesDTO
    {
        public Guid ColorID { get; set; }   
        public Guid SizeID { get; set; }
        public int Quantity { get; set; }

        public string ColorName { get; set; }
        public string SizeName { get; set;}
    }
   
}    

