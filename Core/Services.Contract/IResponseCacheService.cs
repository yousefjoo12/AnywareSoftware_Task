using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Contract
{
    public interface IResponseCacheService
    {
        // Cache Data
        Task CacheResponseAsync(string CacheKey, object Response, TimeSpan ExpireTime);
        // Get Cache Data 
        Task<string> GetCachedResponseAsync(string CacheKeycacheKey);
    }
}
