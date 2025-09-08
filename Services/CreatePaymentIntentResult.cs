namespace EbikeStore.Services
{
    public class CreatePaymentIntentResult
    {
        public string ClientSecret { get; set; } = string.Empty;
        public string PaymentId { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
    }
}