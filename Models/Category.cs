using System.ComponentModel.DataAnnotations;

namespace EbikeStore.Models
{
    /// <summary>
    /// Represents a category for products (e.g., Mountain, City, Folding).
    /// </summary>
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Navigation Property
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}