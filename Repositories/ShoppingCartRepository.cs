using Microsoft.EntityFrameworkCore;
using EbikeStore.Data;
using EbikeStore.Models;

namespace EbikeStore.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ShoppingCart?> GetShoppingCartByUserIdAsync(string userId)
        {
            return await _context.ShoppingCarts
                .Include(sc => sc.CartItems)
                .FirstOrDefaultAsync(sc => sc.ApplicationUserId == userId);
        }

        public async Task<ShoppingCart> CreateShoppingCartAsync(string userId)
        {
            var shoppingCart = new ShoppingCart
            {
                ApplicationUserId = userId
            };
            _context.ShoppingCarts.Add(shoppingCart);
            await _context.SaveChangesAsync();
            return shoppingCart;
        }

        public async Task AddItemToCartAsync(string userId, int productId, int quantity)
        {
            var shoppingCart = await GetShoppingCartByUserIdAsync(userId);
            if (shoppingCart == null)
            {
                shoppingCart = await CreateShoppingCartAsync(userId);
            }

            var existingItem = shoppingCart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var cartItem = new CartItem
                {
                    ShoppingCartId = shoppingCart.Id,
                    ProductId = productId,
                    Quantity = quantity
                };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemFromCartAsync(string userId, int cartItemId)
        {
            var shoppingCart = await GetShoppingCartByUserIdAsync(userId);
            if (shoppingCart != null)
            {
                var cartItem = shoppingCart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
                if (cartItem != null)
                {
                    _context.CartItems.Remove(cartItem);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task UpdateItemQuantityAsync(string userId, int cartItemId, int quantity)
        {
            var shoppingCart = await GetShoppingCartByUserIdAsync(userId);
            if (shoppingCart != null)
            {
                var cartItem = shoppingCart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
                if (cartItem != null)
                {
                    if (quantity <= 0)
                    {
                        _context.CartItems.Remove(cartItem);
                    }
                    else
                    {
                        cartItem.Quantity = quantity;
                    }
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            var shoppingCart = await GetShoppingCartByUserIdAsync(userId);
            if (shoppingCart != null)
            {
                _context.CartItems.RemoveRange(shoppingCart.CartItems);
                await _context.SaveChangesAsync();
            }
        }
    }
}