namespace EbikeStore.Models
{
    public class CreatePaymentIntentResult
    {
        public string? ClientSecret { get; set; }
        public string? PaymentId { get; set; }
        public string? ReturnUrl { get; set; }
    }
}