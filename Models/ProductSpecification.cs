using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EbikeStore.Models
{
    /// <summary>
    /// Represents a single technical specification for a product (e.g., Key: "Motor", Value: "500W").
    /// </summary>
    public class ProductSpecification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Key { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Value { get; set; } = string.Empty;

        // Foreign Key for Product
        public int ProductId { get; set; }

        // Navigation Property
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = new Product();
    }
}