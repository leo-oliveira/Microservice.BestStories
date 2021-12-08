using Microservice.BestStories.Services.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Microservice.BestStories.Services
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private IMemoryCache _cache;
        private IConfiguration _configuration;
        public MemoryCacheService(
            IMemoryCache cache,
            IConfiguration configuration)
        {
            _cache = cache;
            _configuration = configuration;
        }

        public List<StoryDetail> GetCachedStories()
        {
            if (_cache.TryGetValue("cachedStories", out List<StoryDetail> cachedStories))
            {
                return cachedStories;
            }
            return null;
        }

        public void SetCachedStories(List<StoryDetail> stories)
        {
            _cache.Set("cachedStories", stories, new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(
                    Convert.ToDouble(_configuration["MemoryCacheSettings:CacheExpirationInSeconds"])),
                Priority = CacheItemPriority.High
            });
        }
    }
}