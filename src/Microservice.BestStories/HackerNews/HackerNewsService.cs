using Microservice.BestStories.HackerNews.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microservice.BestStories.HackerNews
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly IHttpClientFactory _clientFactory;

        private HttpClient HackerNewsClient
        {
            get
            {
                return _clientFactory.CreateClient("HackerNews");
            }
        }

        public HackerNewsService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<int>> GetFirstBestStoriesIds(int top = 20)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "beststories.json");

            var response = await HackerNewsClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var bestStories = DeserializeJson<IEnumerable<int>>(responseStream);
                return bestStories.Take(top);
            }

            throw new Exception($"Error trying to get the first {top} stories IDs: {response.ReasonPhrase}");
        }

        public async Task<StoryDetail> GetStoryDetailsById(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"item/{id}.json");

            var response = await HackerNewsClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();

                return DeserializeJson<StoryDetail>(responseStream);
            }

            throw new Exception($"Error trying to get story detail. Id: {id}. Error: {response.ReasonPhrase}");
        }

        private T DeserializeJson<T>(Stream stream)
        {
            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);
            var serialize = new JsonSerializer();
            return serialize.Deserialize<T>(jsonReader);
        }
    }
}
