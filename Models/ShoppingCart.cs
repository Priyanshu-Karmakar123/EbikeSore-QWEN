using EbikeStore.Data;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EbikeStore.Models
{
    /// <summary>
    /// Represents the shopping cart for a single user.
    /// </summary>
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }

        // Foreign Key to ApplicationUser (establishes a one-to-one relationship)
        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;

        // Navigation Property
        [ForeignKey("ApplicationUserId")]
        [JsonIgnore]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;
        
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}