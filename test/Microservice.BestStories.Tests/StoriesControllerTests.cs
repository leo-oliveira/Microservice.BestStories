using FluentAssertions;
using Microservice.BestStories.Controllers;
using Microservice.BestStories.HackerNews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Net;

namespace Microservice.BestStories.Tests
{
    [TestFixture]
    public class Tests
    {
        private Mock<ILogger<StoriesController>> mockLogger;
        private Mock<IHackerNewsService> mockHackerNewsService;
        private StoriesController underTest;
        private readonly int bestStoriesDefaultTop = 20;

        [SetUp]
        public void SetUp()
        {
            mockLogger = new Mock<ILogger<StoriesController>>();
            mockHackerNewsService = new Mock<IHackerNewsService>();
            underTest = new StoriesController(mockLogger.Object, mockHackerNewsService.Object);
        }

        [Test]
        public void When_Call_GetAsync_Should_Return_Ok()
        {
            // Act
            var httpResult = underTest.GetAsync();
            var result = httpResult.Result as OkObjectResult;

            // Assert
            mockHackerNewsService.Verify(s => s.GetFirstBestStoriesIds(bestStoriesDefaultTop), Times.Once());

            result.Should().NotBeNull();
            result.StatusCode.Value.Should().Be((int)HttpStatusCode.OK);

        }
    }
}