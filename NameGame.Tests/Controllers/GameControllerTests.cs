using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NameGame.Api.Controllers;
using NameGame.Api.Constants;
using NameGame.Service.Models;
using NameGame.Service.Models.Dto;
using NameGame.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using Xunit;

namespace NameGame.Tests.Controllers {
    public class GameControllerTests {
        private readonly Mock<IGameService> _gameService;
        private readonly Mock<ILogger<GameController>> _logger;
        private readonly string _userId = "12345";
        private readonly string _imageId = "54321";
        private readonly int _challengeId = 1;
        public GameControllerTests() {
            _gameService = new Mock<IGameService>();
            _logger = new Mock<ILogger<GameController>>();
        }

        [Fact]
        public async void Given_valid_NameToFaces_request_it_should_return_200() {
            var request = CreateChallengeRequest();
            var challenge = CreateNameToFacesChallenge();
            _gameService.Setup(x => x.CreateNameToFacesChallengeAsync(request)).ReturnsAsync(challenge);

            var controller = CreateController();
            var actionResult = await controller.NameToFacesChallenge(request).ConfigureAwait(false);

            actionResult.Result.Should().BeOfType(typeof(OkObjectResult));
            ((OkObjectResult)actionResult.Result).StatusCode.Should().Be(200);
            ((OkObjectResult)actionResult.Result).Value.Should().Be(challenge);
        }

        [Fact]
        public async void Given_valid_FaceToNames_request_it_should_return_200() {
            var request = CreateChallengeRequest();
            var challenge = CreateFaceToNamesChallenge();
            _gameService.Setup(x => x.CreateFaceToNamesChallengeAsync(request)).ReturnsAsync(challenge);

            var controller = CreateController();
            var actionResult = await controller.FaceToNamesChallenge(request).ConfigureAwait(false);

            actionResult.Result.Should().BeOfType(typeof(OkObjectResult));
            ((OkObjectResult)actionResult.Result).StatusCode.Should().Be(200);
            ((OkObjectResult)actionResult.Result).Value.Should().Be(challenge);
        }

        [Fact]
        public async void When_creating_new_NameToFaces_challenge_fails_log_error_and_return_500() {
            var request = CreateChallengeRequest();
            var expectedException = new Exception("something went wrong");
            _gameService.Setup(x => x.CreateNameToFacesChallengeAsync(It.IsAny<ChallengeRequest>())).ThrowsAsync(expectedException);

            var controller = CreateController();
            var errorResult = await controller.NameToFacesChallenge(request).ConfigureAwait(false);

            Verify500Response(errorResult, ErrorMessages.CreatingNewChallenge, expectedException);
        }

        [Fact]
        public async void When_creating_new_FaceToNames_challenge_fails_log_error_and_return_500() {
            var request = CreateChallengeRequest();
            var expectedException = new Exception("something went wrong");
            _gameService.Setup(x => x.CreateFaceToNamesChallengeAsync(It.IsAny<ChallengeRequest>())).ThrowsAsync(expectedException);

            var controller = CreateController();
            var errorResult = await controller.FaceToNamesChallenge(request).ConfigureAwait(false);

            Verify500Response(errorResult, ErrorMessages.CreatingNewChallenge, expectedException);
        }

        [Fact]
        public async void When_verifying_NameToFaces_asnwer_fails_log_error_and_return_500() {
            var answer = CreateChallengeAnswer(_challengeId, _imageId);
            var expectedException = new Exception("something went wrong");
            _gameService.Setup(x => x.IsAnswerValidAsync(answer)).ThrowsAsync(expectedException);

            var controller = CreateController();
            var errorResult = await controller.NameToFacesChallenge(answer).ConfigureAwait(false);

            Verify500Response(errorResult, ErrorMessages.VerifyingAnswer, expectedException);
        }

        [Fact]
        public async void When_verifying_FaceToNames_asnwer_fails_log_error_and_return_500() {
            var answer = CreateChallengeAnswer(_challengeId, _imageId);
            var expectedException = new Exception("something went wrong");
            _gameService.Setup(x => x.IsAnswerValidAsync(answer)).ThrowsAsync(expectedException);

            var controller = CreateController();
            var errorResult = await controller.FaceToNamesChallenge(answer).ConfigureAwait(false);

            Verify500Response(errorResult, ErrorMessages.VerifyingAnswer, expectedException);
        }

        [Fact]
        public async void Given_null_answer_for_NameToFaces_it_should_return_400_and_warn_about_answer() {
            ChallengeAnswer answer = null;
            var logMessage = string.Format(ErrorMessages.ValueIsEmpty, nameof(answer));
            var controller = CreateController();
            var result = await controller.NameToFacesChallenge(answer).ConfigureAwait(false);

            Verify400Result(result, logMessage);
        }

        [Fact]
        public async void Given_empty_ChallengeId_for_NameToFaces_it_should_return_400_and_warn_about_ChallengeId() {
            var answer = CreateChallengeAnswer(0, _imageId);
            var logMessage = string.Format(ErrorMessages.ValueIsEmpty, nameof(answer.ChallengeId));
            var controller = CreateController();
            var result = await controller.NameToFacesChallenge(answer).ConfigureAwait(false);

            Verify400Result(result, logMessage);
        }

        [Fact]
        public async void Given_empty_GivenAnswer_for_NameToFaces_it_should_return_400_and_warn_about_GivenAnswer() {
            var answer = CreateChallengeAnswer(_challengeId);
            var logMessage = string.Format(ErrorMessages.ValueIsEmpty, nameof(answer.GivenAnswer));
            var controller = CreateController();
            var result = await controller.NameToFacesChallenge(answer).ConfigureAwait(false);

            Verify400Result(result, logMessage);
        }

        [Fact]
        public async void Given_null_answer_for_FaceToNames_it_should_return_400_and_warn_about_answer() {
            ChallengeAnswer answer = null;
            var logMessage = string.Format(ErrorMessages.ValueIsEmpty, nameof(answer));
            var controller = CreateController();
            var result = await controller.FaceToNamesChallenge(answer).ConfigureAwait(false);

            Verify400Result(result, logMessage);
        }

        [Fact]
        public async void Given_empty_ChallengeId_for_FaceToNames_it_should_return_400_and_warn_about_ChallengeId() {
            var answer = CreateChallengeAnswer(0, _imageId);
            var logMessage = string.Format(ErrorMessages.ValueIsEmpty, nameof(answer.ChallengeId));
            var controller = CreateController();
            var result = await controller.FaceToNamesChallenge(answer).ConfigureAwait(false);

            Verify400Result(result, logMessage);
        }

        [Fact]
        public async void Given_empty_GivenAnswer_for_FaceToNames_it_should_return_400_and_warn_about_GivenAnswer() {
            var answer = CreateChallengeAnswer(_challengeId);
            var logMessage = string.Format(ErrorMessages.ValueIsEmpty, nameof(answer.GivenAnswer));
            var controller = CreateController();
            var result = await controller.FaceToNamesChallenge(answer).ConfigureAwait(false);

            Verify400Result(result, logMessage);
        }

        private GameController CreateController() {
            return new GameController(_gameService.Object, _logger.Object);
        }

        private NameToFacesChallenge CreateNameToFacesChallenge() {
            return new NameToFacesChallenge(
                _challengeId,
                "description",
                new Employee(_userId, "John", "Doe"),
                new List<Face>() { new Face(_imageId, "http://someurl.com/mypic.png") }.ToArray());
        }

        private FaceToNamesChallenge CreateFaceToNamesChallenge() {
            return new FaceToNamesChallenge(
                _challengeId,
                "description",
                new List<Employee>() { new Employee(_userId, "John", "Doe") }.ToArray(),
                new Face(_imageId, "http://someurl.com/mypic.png"));
        }

        private ChallengeAnswer CreateChallengeAnswer(int challengeId = 0, string givenAnswer = null) {
            return new ChallengeAnswer() {
                ChallengeId = challengeId,
                GivenAnswer = givenAnswer
            };
        }

        private ChallengeRequest CreateChallengeRequest() {
            return new ChallengeRequest() {
                UserName = "Someone",
                NumberOfOptions = 6
            };
        }

        private void Verify500Response<T>(ActionResult<T> errorResult, string error, Exception expectedException) {
            errorResult.Result.Should().BeOfType<ObjectResult>();
            ((ObjectResult)errorResult.Result).StatusCode.Should().Be(500);
            ((ObjectResult)errorResult.Result).Value.Should().Be(error);
        }

        private void Verify400Result<T>(ActionResult<T> result, string logValue) {
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            ((BadRequestObjectResult)result.Result).StatusCode.Should().Be(400);
            ((BadRequestObjectResult)result.Result).Value.Should().Be(logValue);

            _gameService.Verify(x =>
                    x.IsAnswerValidAsync(It.IsAny<ChallengeAnswer>()),
                    Times.Never());
        }
    }
}