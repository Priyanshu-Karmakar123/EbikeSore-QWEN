using EbikeStore.Models;

namespace EbikeStore.Services
{
    public interface IStripeService
    {
        Task<CreatePaymentIntentResult> CreatePaymentIntentAsync(string customerId, int orderId, decimal amount, string returnUrl, string paymentMethodType);
    }
}