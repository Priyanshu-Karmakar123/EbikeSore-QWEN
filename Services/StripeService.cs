using Microsoft.Extensions.Configuration;
using Stripe;
using EbikeStore.Models;

namespace EbikeStore.Services
{
    public class StripeService : IStripeService
    {
        private readonly IConfiguration _configuration;

        public StripeService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        public async Task<CreatePaymentIntentResult> CreatePaymentIntentAsync(string customerId, int orderId, decimal amount, string returnUrl, string paymentMethodType)
        {
            try
            {
                // Validate payment method type
                if (!new[] { "cashapp", "card" }.Contains(paymentMethodType))
                {
                    throw new ArgumentException($"Invalid payment method type: {paymentMethodType}");
                }

                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(amount * 100), // Convert to cents
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { paymentMethodType }, // Set specific payment method
                    Metadata = new Dictionary<string, string>
                    {
                        { "OrderId", orderId.ToString() },
                        { "CustomerId", customerId },
                        { "PaymentMethodType", paymentMethodType }
                    }
                };

                var service = new PaymentIntentService();
                var paymentIntent = await service.CreateAsync(options);

                return new CreatePaymentIntentResult
                {
                    ClientSecret = paymentIntent.ClientSecret,
                    PaymentId = paymentIntent.Id,
                    ReturnUrl = returnUrl
                };
            }
            catch (StripeException ex)
            {
                throw new InvalidOperationException($"Failed to create PaymentIntent: {ex.Message}", ex);
            }
        }
    }
}