namespace EbikeStore.Models
{
    /// <summary>
    /// Enumeration for the possible statuses of an order.
    /// </summary>
    public enum OrderStatus
    {
        Pending,        // Order placed, but payment not confirmed
        Processing,     // Payment confirmed, order is being prepared
        Shipped,        // Order has been dispatched
        Delivered,      // Order has been received by the customer
        Cancelled,      // Order was cancelled
        Refunded        // Order was returned and refunded
    }
}