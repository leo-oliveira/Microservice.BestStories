using Microservice.BestStories.HackerNews;
using Microservice.BestStories.HackerNews.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Microservice.BestStories.Controllers
{
    [ApiController]
    [Route("/best20")]
    [Produces("application/json")]
    public class StoriesController : ControllerBase
    {
        private readonly ILogger<StoriesController> _logger;
        private readonly IHackerNewsService _hackerNewsService;

        public StoriesController(ILogger<StoriesController> logger,
            IHackerNewsService hackerNewsService)
        {
            _logger = logger;
            _hackerNewsService = hackerNewsService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StoryDetail>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAsync(int top = 20)
        {
            try
            {
                if (top > 500) return BadRequest("The number of stories cannot be greater than 500");

                _logger.LogInformation($"Getting {top} best stories");
                var storyTasks = new List<Task<StoryDetail>>();
                var bestTwentyStoriesIds = await _hackerNewsService.GetFirstBestStoriesIds(top);
                foreach (var id in bestTwentyStoriesIds)
                {
                    storyTasks.Add(_hackerNewsService.GetStoryDetailsById(id));
                }
                await Task.WhenAll(storyTasks);

                var results = new List<StoryDetail>();
                foreach (var task in storyTasks)
                {
                    results.Add(task.Result);
                }
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error trying to get best stories: {ex}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Your request could not be processed. Try again. Error: {ex.Message}");
            }
        }
    }
}
