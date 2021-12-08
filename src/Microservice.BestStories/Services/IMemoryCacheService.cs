using Microservice.BestStories.Services.Models;
using System.Collections.Generic;

namespace Microservice.BestStories.Services
{
    public interface IMemoryCacheService
    {
        List<StoryDetail> GetCachedStories();
        void SetCachedStories(List<StoryDetail> stories);
    }
}