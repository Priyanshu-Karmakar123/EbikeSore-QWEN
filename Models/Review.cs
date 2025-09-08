using EbikeStore.Data;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
 

namespace EbikeStore.Models
{
    /// <summary>
    /// Represents a customer review for a product.
    /// </summary>
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } // e.g., 1 to 5 stars

        public string? Comment { get; set; }

        [Required]
        public DateTime ReviewDate { get; set; }

        // Foreign Keys
        public int ProductId { get; set; }
        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;

        // Navigation Properties
        [ForeignKey("ProductId")]
        [JsonIgnore]
        public virtual Product Product { get; set; } = new Product();

        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; } = new ApplicationUser();
    }
}