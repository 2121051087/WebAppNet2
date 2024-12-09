using System.ComponentModel.DataAnnotations;

namespace WebAppNet2.Models.Entities.Orders
{
    public class Orders
    {
        [Key]
        public Guid OrderID { get; set; }
        public Guid UserID { get; set; }

        public DateTime OrderDate { get; set; }

        public string FullName { get; set; }

        public double TotalAmount { get; set; }

        public string Status { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public string Ward { get; set; }

        public string PhoneNumber { get; set; }

        // 

        //public ApplicationUser applicationUser { get; set;} = new ApplicationUser();

        public ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
    }
}
