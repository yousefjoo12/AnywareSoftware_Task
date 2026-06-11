using Core.Services.Contract; 
using StackExchange.Redis; 
using System.Text.Json; 

namespace Service
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _database;

        public ResponseCacheService(IConnectionMultiplexer Redis)
        {
            _database = Redis.GetDatabase();
        }
        public async Task CacheResponseAsync(string CacheKey, object Response, TimeSpan ExpireTime)
        {
            if (Response == null) return;
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var serializedResponse = JsonSerializer.Serialize(Response, options);
            await _database.StringSetAsync(CacheKey, serializedResponse, ExpireTime);
        }

        public async Task<string?> GetCachedResponseAsync(string CacheKey)
        {
            var cachedResponse = await _database.StringGetAsync(CacheKey);
            if (cachedResponse.IsNullOrEmpty) return null;
            return cachedResponse;
        }
    }
}
