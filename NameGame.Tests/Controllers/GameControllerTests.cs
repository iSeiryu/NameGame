using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Moq;
using NameGame.API.Constants;
using NameGame.API.Controllers;
using NameGame.Service.Models;
using NameGame.Service.Models.Dto;
using NameGame.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using Xunit;

namespace NameGame.Tests.Controllers
{
    public class GameControllerTests
    {
        private readonly Mock<IGameService> _gameService;
        private readonly Mock<ILogger<GameController>> _logger;
        private readonly string _userId = "12345";
        private readonly string _imageId = "54321";

        public GameControllerTests()
        {
            _gameService = new Mock<IGameService>();
            _logger = new Mock<ILogger<GameController>>();
        }

        [Fact]
        public async void Given_valid_request_it_should_return_200()
        {
            var request = new ChallengeRequest();
            var challenge = CreateChallenge();
            _gameService.Setup(x => x.CreateNameToFacesChallengeAsync(request)).ReturnsAsync(challenge);

            var controller = CreateController();
            var actionResult = await controller.NameToFacesChallenge(request).ConfigureAwait(false);

            actionResult.Result.Should().BeOfType(typeof(OkObjectResult));
            ((OkObjectResult)actionResult.Result).StatusCode.Should().Be(200);
            ((OkObjectResult)actionResult.Result).Value.Should().Be(challenge);
        }

        [Fact]
        public async void When_creating_new_challenge_fails_log_error_and_return_500()
        {
            var expectedException = new Exception("something went wrong");
            _gameService.Setup(x => x.CreateNameToFacesChallengeAsync(It.IsAny<ChallengeRequest>())).ThrowsAsync(expectedException);

            var controller = CreateController();
            var errorResult = await controller.NameToFacesChallenge(new ChallengeRequest()).ConfigureAwait(false);

            Verify500Response(errorResult, ErrorMessages.CreatingNewChallenge, expectedException);
        }

        [Fact]
        public async void When_verifying_asnwer_fails_log_error_and_return_500()
        {
            var answer = new ChallengeAnswer() { GivenUserId = _userId, SelectedImageId = _imageId };
            var expectedException = new Exception("something went wrong");
            _gameService.Setup(x => x.IsAnswerValidAsync(answer)).ThrowsAsync(expectedException);

            var controller = CreateController();
            var errorResult = await controller.NameToFacesChallenge(answer).ConfigureAwait(false);

            Verify500Response(errorResult, ErrorMessages.VerifyingAnswer, expectedException);
        }

        [Fact]
        public async void Given_null_answer_it_should_return_400_and_warn_about_answer()
        {
            ChallengeAnswer answer = null;
            var logMessage = string.Format(ErrorMessages.ValueIsEmpty, nameof(answer));
            var controller = CreateController();
            var result = await controller.NameToFacesChallenge(answer).ConfigureAwait(false);

            Verify400Result(result, logMessage);
        }

        [Fact]
        public async void Given_empty_userId_it_should_return_400_and_warn_about_GivenUserId()
        {
            ChallengeAnswer answer = new ChallengeAnswer() { SelectedImageId = _imageId };
            var logMessage = string.Format(ErrorMessages.ValueIsEmpty, nameof(answer.GivenUserId));
            var controller = CreateController();
            var result = await controller.NameToFacesChallenge(answer).ConfigureAwait(false);

            Verify400Result(result, logMessage);
        }

        [Fact]
        public async void Given_empty_imageId_it_should_return_400_and_warn_about_SelectedImageId()
        {
            ChallengeAnswer answer = new ChallengeAnswer() { GivenUserId = _userId };
            var logMessage = string.Format(ErrorMessages.ValueIsEmpty, nameof(answer.SelectedImageId));
            var controller = CreateController();
            var result = await controller.NameToFacesChallenge(answer).ConfigureAwait(false);

            Verify400Result(result, logMessage);
        }

        private GameController CreateController()
        {
            return new GameController(_gameService.Object, _logger.Object);
        }

        private Challenge CreateChallenge()
        {
            return new Challenge(
                "description", 
                new Employee(_userId, "John", "Doe"),
                new List<Face>() { new Face(_imageId, "http://someurl.com/mypic.png") }.ToArray());
        }

        private void Verify500Response<T>(ActionResult<T> errorResult, string error, Exception expectedException)
        {
            errorResult.Result.Should().BeOfType<ObjectResult>();
            ((ObjectResult)errorResult.Result).StatusCode.Should().Be(500);
            ((ObjectResult)errorResult.Result).Value.Should().Be(error);

            _logger.Verify(m => m.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<FormattedLogValues>(),
                expectedException,
                It.IsAny<Func<object, Exception, string>>()));
        }

        private void Verify400Result<T>(ActionResult<T> result, string logValue)
        {
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            ((BadRequestObjectResult)result.Result).StatusCode.Should().Be(400);
            ((BadRequestObjectResult)result.Result).Value.Should().Be(logValue);

            _gameService.Verify(x =>
                    x.IsAnswerValidAsync(It.IsAny<ChallengeAnswer>()),
                    Times.Never());

            _logger.Verify(m => m.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<FormattedLogValues>(x => x.ToString().Equals(logValue)),
                null,
                It.IsAny<Func<object, Exception, string>>()));
        }
    }
}
