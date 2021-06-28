using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NameGame.Api.Constants;
using NameGame.Service.Models;
using NameGame.Service.Models.Dto;
using NameGame.Service.Services.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NameGame.Api.Controllers {
    /// <summary>
    /// A set of endpoints to allow to build the Name Game
    /// </summary>
    [Route("[action]")]
    [ApiController]
    public class ChallengeController : ControllerBase {
        private readonly IGameResourceService _gameService;
        private readonly ILogger<ChallengeController> _logger;

        public ChallengeController(IGameResourceService gameService, ILogger<ChallengeController> logger) {
            _gameService = gameService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all created challenges.
        /// </summary>
        /// <returns>Challenge objects.</returns>
        [HttpGet("/[controller]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> GetAll() {
            try {
                return Ok(await _gameService.GetChallenges().ConfigureAwait(false));
            }
            catch (Exception ex) {
                _logger.LogError(ex, ErrorMessages.CreatingNewChallenge);
                return StatusCode((int)HttpStatusCode.InternalServerError, ErrorMessages.GeneralGameError);
            }
        }

        /// <summary>
        /// Gets a specific challenge.
        /// </summary>
        /// <param name="challengeId"></param>
        /// <returns>Challenge object.</returns>
        [HttpGet("/[controller]/{challengeId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Get(int challengeId) {
            try {
                if (challengeId < 1) return BadRequest(BuildErrorMessage(nameof(challengeId)));

                var challenge = await _gameService.GetChallenge(challengeId).ConfigureAwait(false);
                if (challenge == null) return NotFound();

                return Ok(challenge);
            }
            catch (Exception ex) {
                _logger.LogError(ex, ErrorMessages.CreatingNewChallenge);
                return StatusCode((int)HttpStatusCode.InternalServerError, ErrorMessages.GeneralGameError);
            }
        }

        /// <summary>
        /// Creates a new challenge of identifing the listed name.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Challenge object.</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<NameToFacesChallenge>> NameToFacesChallenge([FromBody] ChallengeRequest request) {
            try {
                var (success, errorMessage) = ValidateRequest(request);
                if (!success) {
                    _logger.LogWarning(errorMessage);
                    return BadRequest(errorMessage);
                }

                var newChallenge = await _gameService.CreateNameToFacesChallenge(request).ConfigureAwait(false);
                return Ok(newChallenge);
            }
            catch (Exception ex) {
                _logger.LogError(ex, ErrorMessages.CreatingNewChallenge);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Checks if the answer is correct.
        /// </summary>
        /// <param name="answer"></param>
        /// <returns>A validation result with either success or error message.</returns>
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ChallengeAnswerValidationResult>> NameToFacesChallenge([FromBody] ChallengeAnswer answer) {
            try {
                var (success, errorMessage) = ValidateAnswer(answer);
                if (!success) {
                    _logger.LogWarning(errorMessage);
                    return BadRequest(errorMessage);
                }

                return await _gameService.IsAnswerValid(answer).ConfigureAwait(false);
            }
            catch (Exception ex) {
                _logger.LogError(ex, ErrorMessages.VerifyingAnswer);
                return StatusCode((int)HttpStatusCode.InternalServerError, ErrorMessages.VerifyingAnswer);
            }
        }

        /// <summary>
        /// Gets a new challenge of identifing the listed face.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Challenge object.</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<NameToFacesChallenge>> FaceToNamesChallenge([FromBody] ChallengeRequest request) {
            try {
                var (success, errorMessage) = ValidateRequest(request);
                if (!success) {
                    _logger.LogWarning(errorMessage);
                    return BadRequest(errorMessage);
                }

                var newChallenge = await _gameService.CreateFaceToNamesChallenge(request);
                return Ok(newChallenge);
            }
            catch (Exception ex) {
                _logger.LogError(ex, ErrorMessages.CreatingNewChallenge);
                return StatusCode((int)HttpStatusCode.InternalServerError, ErrorMessages.CreatingNewChallenge);
            }
        }

        /// <summary>
        /// Checks if the answer is correct.
        /// </summary>
        /// <param name="answer"></param>
        /// <returns>A validation result with either success or error message.</returns>
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ChallengeAnswerValidationResult>> FaceToNamesChallenge([FromBody] ChallengeAnswer answer) {
            try {
                var (success, errorMessage) = ValidateAnswer(answer);
                if (!success) {
                    _logger.LogWarning(errorMessage);
                    return BadRequest(errorMessage);
                }

                return await _gameService.IsAnswerValid(answer).ConfigureAwait(false);
            }
            catch (Exception ex) {
                _logger.LogError(ex, ErrorMessages.VerifyingAnswer);
                return StatusCode((int)HttpStatusCode.InternalServerError, ErrorMessages.VerifyingAnswer);
            }
        }

        /// <summary>
        /// Deletes a specific challenge.
        /// </summary>
        /// <returns>Success of failure.</returns>
        [HttpDelete("/[controller]/{challengeId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Delete(int challengeId) {
            try {
                var challenge = await _gameService.GetChallenge(challengeId).ConfigureAwait(false);
                if (challenge == null) return NotFound();

                return Ok(await _gameService.DeleteChallenge(challenge).ConfigureAwait(false));
            }
            catch (Exception ex) {
                _logger.LogError(ex, ErrorMessages.CreatingNewChallenge);
                return StatusCode((int)HttpStatusCode.InternalServerError, ErrorMessages.GeneralGameError);
            }
        }

        private (bool, string) ValidateRequest(ChallengeRequest request) {
            if (request == null)
                return (false, BuildErrorMessage(nameof(request)));

            if (string.IsNullOrEmpty(request.UserName))
                return (false, BuildErrorMessage(nameof(request.UserName)));

            if (request.NumberOfOptions < 1)
                return (false, BuildErrorMessage(nameof(request.NumberOfOptions)));

            return (true, null);
        }

        private (bool, string) ValidateAnswer(ChallengeAnswer answer) {
            if (answer == null)
                return (false, BuildErrorMessage(nameof(answer)));

            if (answer.ChallengeId < 1)
                return (false, BuildErrorMessage(nameof(answer.ChallengeId)));

            if (string.IsNullOrEmpty(answer.GivenAnswer))
                return (false, BuildErrorMessage(nameof(answer.GivenAnswer)));

            return (true, null);
        }

        private string BuildErrorMessage(string parameter) {
            return string.Format(ErrorMessages.ValueIsEmpty, parameter);
        }
    }
}
