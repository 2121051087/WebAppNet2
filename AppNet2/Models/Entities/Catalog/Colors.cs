using AppNet2.Models;
using System.ComponentModel.DataAnnotations;

namespace WebAppNet2.Models.Entities.Catalog
{
    public class Colors
    {
        [Key]
        public Guid ColorID { get; set; }

        public string ColorName { get; set; }

        public ICollection<ColorSizes> ColorSizes { get; set; }

        public static void SeedDefaultColors(DbNet2Context context)
        {
            if (!context.Colors.Any())
            {
                var colorNames = new string[] { "Red", "Blue", "White", "Black", "Yellow", "Green", "Purple", "Pink", "Orange", "Brown", "Gray", "Silver", "Gold" };

                var defaultColors = new List<Colors>();

                foreach (var colorName in colorNames)
                {
                    defaultColors.Add(new Colors
                    {

                        ColorName = colorName,

                    });
                }

                context.Colors.AddRange(defaultColors);
                context.SaveChanges();
            }
        }
    }
}
