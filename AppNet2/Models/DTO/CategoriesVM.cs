using System.ComponentModel;

namespace WebAppNet2.Models.DTO
{
    public class CategoriesVM
    {
        public Guid? CategoryID { get; set; }

        [DisplayName("Tên danh mục")]
        public string CategoryName { get; set; } = null!;
        public int? CountNumberProductByCategory { get; set; } = 0;

        public List<ProductsVM>? Products { get; set; }

    }
}
