using System.ComponentModel.DataAnnotations;

namespace EbikeStore.Models
{
    /// <summary>
    /// Represents the brand or manufacturer of the electric bikes.
    /// </summary>
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? LogoUrl { get; set; }

        // Navigation Property
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}