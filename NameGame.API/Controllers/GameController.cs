using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NameGame.Domain.Models;
using NameGame.Domain.Models.Dto;
using NameGame.Domain.Services.Interfaces;
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
                var newChallenge = await _gameService.CreateNameToFacesChallenge(request);
                return Ok(newChallenge);
            }
            catch(Exception ex)
            {
                const string errorMessage = "An error occured while creating a new challenge.";
                _logger.LogError(ex, errorMessage);
                return StatusCode((int)HttpStatusCode.InternalServerError, errorMessage);
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
                return await _gameService.IsAnswerValid(answer).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                const string errorMessage = "An error occured while verifying the answer.";
                _logger.LogError(ex, errorMessage);
                return StatusCode((int)HttpStatusCode.InternalServerError, errorMessage);
            }
        }
    }
}
