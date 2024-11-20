using AppNet2.Models;
using System.ComponentModel.DataAnnotations;

namespace WebAppNet2.Models.Entities.Catalog
{
    public class Sizes
    {
        [Key]

        public Guid SizeID { get; set; }

        public string SizeName { get; set; }


        public ICollection<ColorSizes> ColorSizes { get; set; }

        public static void SeedDefaultSizes(DbNet2Context context)
        {
            if (!context.Sizes.Any())
            {
                var sizeNames = new string[] { "X", "M", "XL", "L", "S" };

                var defaultSizes = new List<Sizes>();

                foreach (var size in sizeNames)
                {
                    defaultSizes.Add(new Sizes
                    {

                        SizeName = size,

                    });
                }

                context.Sizes.AddRange(defaultSizes);
                context.SaveChanges();
            }
        }

    }
}
