using Evently.Common.Application.Caching;

namespace Evently.Modules.Ticketing.Application.Carts;

public sealed class CartService(ICacheService cache)
{
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(20);

    public async Task<Cart> GetAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var cacheKey = CreateCacheKey(customerId);
        
        var cart = await cache.GetAsync<Cart>(cacheKey, cancellationToken) ?? Cart.CreateDefault(customerId);

        return cart;
    }

    public async Task ClearAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var cacheKey = CreateCacheKey(customerId);

        var cart = Cart.CreateDefault(customerId);

        await cache.SetAsync(cacheKey, cart, DefaultExpiration, cancellationToken);
    }
    
    public async Task AddItemAsync(Guid customerId, CartItem cartItem, CancellationToken cancellationToken = default)
    {
        var cacheKey = CreateCacheKey(customerId);
        
        var cart = await GetAsync(customerId, cancellationToken);
        
        var existingCartIem = cart.Items.Find(item => item.TicketTypeId == cartItem.TicketTypeId);
        
        if (existingCartIem is null)
            cart.Items.Add(cartItem);
        else
            existingCartIem.Quantity += cartItem.Quantity;
        
        await cache.SetAsync(cacheKey, cart, DefaultExpiration, cancellationToken);
    }

    public async Task RemoveItemAsync(Guid customerId, Guid ticketTypeId, CancellationToken cancellationToken = default)
    {
        var cacheKey = CreateCacheKey(customerId);

        var cart = await GetAsync(customerId, cancellationToken);

        var cartItem = cart.Items.Find(item => item.TicketTypeId == ticketTypeId);

        if (cartItem is null) return;
        
        cart.Items.Remove(cartItem);
        
        await cache.SetAsync(cacheKey, cart, DefaultExpiration, cancellationToken);
    }
    
    private static string CreateCacheKey(Guid customerId) => $"carts:{customerId}";
    
}