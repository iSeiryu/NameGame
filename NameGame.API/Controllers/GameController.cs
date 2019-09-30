using Microsoft.AspNetCore.Mvc;
using NameGame.Domain.Models;
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

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
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
                var newChallenge = await _gameService.CreateChallenge(request);
                return Ok(newChallenge);
            }
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occured while creating new challenge.");
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
        public ActionResult<bool> NameToFacesChallenge([FromBody] ChallengeAnswer answer)
        {
            var result = _gameService.IsAnswerValid(answer);
            return result;
        }
    }
}
