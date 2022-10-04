using System;
using System.Text.Json;
using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Api.Repositories
{
    public class BasketRepository : IBasketRepository
    {

        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache cache)
        {
            _redisCache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task DeleteBasket(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);
            if (basket != null)
            {
                await _redisCache.RemoveAsync(userName);
            }
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);
            if (string.IsNullOrEmpty(basket))
                return null;

            return JsonSerializer.Deserialize<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await _redisCache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket));
            return await GetBasket(basket.UserName);
        }
    }
}

