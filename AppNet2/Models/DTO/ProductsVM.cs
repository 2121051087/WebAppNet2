using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAppNet2.Models.DTO
{
    public class ProductsVM
    {
        public Guid? ProductID { get; set; }

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

        [DisplayName("Ngày cập nhật")]
        public DateTime? UpdatedAt { get; set; }
        [DisplayName("Danh mục")]
        public string? CategoryName { get; set; }

        [DisplayName("Thông số")]
        public List<ColorSizesDTO>? colorSizesDTO { get; set; } 

        public List<IGrouping<Guid,ColorSizesDTO>>? groupColorSizeByColoRID { get; set; }


        // Thêm thuộc tính cho dữ liệu JSON để gửi xuống View
        //public string? ColorSizesDTOJson { get; set; }

    }

    public class ColorSizesDTO
    {
        public Guid? ColorSizesID { get; set; }
        public Guid ColorID { get; set; }   
        public Guid SizeID { get; set; }
        public int Quantity { get; set; }

        public string? ColorHexCode { get; set; }
        public string? ColorName { get; set; }
        public string? SizeName { get; set; }
    }
   
}    

