using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NameGame.API.Constants;
using NameGame.Service.Models;
using NameGame.Service.Models.Dto;
using NameGame.Service.Services.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NameGame.API.Controllers
{
    /// <summary>
    /// A set of endpoints to allow to build the Name Game
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly ILogger<GameController> _logger;

        public GameController(IGameService gameService, ILogger<GameController> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        /// <summary>
        /// Get a new challenge of identifing the listed name.
        /// </summary>
        /// <returns>Challenge object.</returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<Challenge>> NameToFacesChallenge([FromQuery] ChallengeRequest request)
        {
            try
            {
                var newChallenge = await _gameService.CreateNameToFacesChallengeAsync(request);
                return Ok(newChallenge);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.CreatingNewChallenge);
                return StatusCode((int)HttpStatusCode.InternalServerError, ErrorMessages.CreatingNewChallenge);
            }
        }

        /// <summary>
        /// Check if the answer is correct.
        /// </summary>
        /// <param name="answer"></param>
        /// <returns>True or false.</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<bool>> NameToFacesChallenge([FromBody] ChallengeAnswer answer)
        {
            try
            {
                var (success, errorMessage) = ValidateAnswer(answer);
                if (!success)
                {
                    _logger.LogWarning(errorMessage);
                    return BadRequest(errorMessage);
                }

                return await _gameService.IsAnswerValidAsync(answer).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.VerifyingAnswer);
                return StatusCode((int)HttpStatusCode.InternalServerError, ErrorMessages.VerifyingAnswer);
            }
        }

        private (bool, string) ValidateAnswer(ChallengeAnswer answer)
        {
            if(answer == null)
                return (false, BuildErrorMessage(nameof(answer)));

            if (string.IsNullOrEmpty(answer.GivenUserId))
                return (false, BuildErrorMessage(nameof(answer.GivenUserId)));

            if (string.IsNullOrEmpty(answer.SelectedImageId))
                return (false, BuildErrorMessage(nameof(answer.SelectedImageId)));

            return (true, null);

            string BuildErrorMessage(string parameter)
            {
                return string.Format(ErrorMessages.ValueIsEmpty, parameter);
            }
        }
    }
}
