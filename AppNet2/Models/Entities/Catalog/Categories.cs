using System.ComponentModel.DataAnnotations;

namespace WebAppNet2.Models.Entities.Catalog
{
    public class Categories
    {

        [Key]
        public Guid CategoryID { get; set; }

        [Required(ErrorMessage = "Category Name is required.")]
        public string CategoryName { get; set; }

        public ICollection<Products> Products { get; set; } = new List<Products>();
    }
}
