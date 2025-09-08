using EbikeStore.Data;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EbikeStore.Models
{
    /// <summary>
    /// Represents a customer's order.
    /// </summary>
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        // Foreign Keys
        [Required]
        [JsonIgnore]
        public string ApplicationUserId { get; set; } = string.Empty;

        public int ShippingAddressId { get; set; }

        public int BillingAddressId { get; set; }

        // Navigation Properties
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; } = new ApplicationUser();

        [ForeignKey("ShippingAddressId")]
        public virtual Address ShippingAddress { get; set; } = new Address();

        [ForeignKey("BillingAddressId")]
        public virtual Address BillingAddress { get; set; } = new Address();

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}