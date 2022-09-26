using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace RedisDemo.Extensions
{
    public static class DistributedCacheExtension
    {
        public static async Task SetRecoredAsync<T>(this IDistributedCache cache, 
            string recordId, 
            T data, 
            TimeSpan? absoluteExpireTime=null, 
            TimeSpan? UnUsedExpireTIme=null)
        {
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
            options.SlidingExpiration=UnUsedExpireTIme;
            
            var jsonData=JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recordId, jsonData, options);
        }

        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
        {
            var JsonData= await cache.GetStringAsync(recordId);
            if(JsonData == null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(JsonData);

        }
    }
}
