using AppNet2.Models;
using System.ComponentModel.DataAnnotations;

namespace WebAppNet2.Models.Entities.Catalog
{
    public class Colors
    {
        [Key]
        public Guid ColorID { get; set; }

        public string ColorName { get; set; }

        public string? ColorHexCode { get; set; }

        public ICollection<ColorSizes> ColorSizes { get; set; }

        public static void SeedDefaultColors(DbNet2Context context)
        {
            if (!context.Colors.Any())
            {
                var colorData = new (string ColorName, string ColorHexCode)[]
                {
            ("Red", "#FF0000"),
            ("Blue", "#0000FF"),
            ("White", "#FFFFFF"),
            ("Black", "#000000"),
            ("Yellow", "#FFFF00"),
            ("Green", "#008000"),
            ("Purple", "#800080"),
            ("Pink", "#FFC0CB"),
            ("Orange", "#FFA500"),
            ("Brown", "#A52A2A"),
            ("Gray", "#808080"),
            ("Silver", "#C0C0C0"),
            ("Gold", "#FFD700")
                };

                var defaultColors = new List<Colors>();

                foreach (var color in colorData)
                {
                    defaultColors.Add(new Colors
                    {
                        ColorName = color.ColorName,
                        ColorHexCode = color.ColorHexCode
                    });
                }

                context.Colors.AddRange(defaultColors);
                context.SaveChanges();
            }
        }

    }
}
