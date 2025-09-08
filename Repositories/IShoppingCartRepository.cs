using EbikeStore.Models;

namespace EbikeStore.Repositories
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart?> GetShoppingCartByUserIdAsync(string userId);
        Task<ShoppingCart> CreateShoppingCartAsync(string userId);
        Task AddItemToCartAsync(string userId, int productId, int quantity);
        Task RemoveItemFromCartAsync(string userId, int cartItemId);
        Task UpdateItemQuantityAsync(string userId, int cartItemId, int quantity);
        Task ClearCartAsync(string userId);
    }
}