using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NameGame.API.Constants;
using NameGame.Service.Services.Interfaces;
using System;
using System.Net;

namespace NameGame.API.Controllers
{
    /// <summary>
    /// A set of endpoints to get the Name Game statistics
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;
        private readonly ILogger<GameController> _logger;

        public StatisticsController(IStatisticsService statisticsService, ILogger<GameController> logger)
        {
            _statisticsService = statisticsService;
            _logger = logger;
        }

        /// <summary>
        /// Gets the number of attempts the user made for the last challenge.
        /// </summary>
        /// <param name="userName">Username of the user that attempted to solve the last challenge.</param>
        /// <returns>Number of attempts.</returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult<int> GetLastAttempts(string userName)
        {
            try
            {
                var (success, errorMessage) = ValidateInput(userName);
                if (!success)
                {
                    _logger.LogWarning(errorMessage);
                    return BadRequest(errorMessage);
                }

                return Ok(_statisticsService.GetLastAttempts(userName));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.SomethingWentWrongWithStatistics);
                return StatusCode((int)HttpStatusCode.InternalServerError, ErrorMessages.SomethingWentWrongWithStatistics);
            }
        }

        /// <summary>
        /// Gets the time length that took to solve the last challenge.
        /// </summary>
        /// <param name="userName">Username of the user that solved the last challenge.</param>
        /// <returns>Time duration between the creation of the challenge and marking it solved. It returns null when no solved challenges were found.</returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult<TimeSpan?> HowLongDidLastChallengeTake(string userName)
        {
            try
            {
                var (success, errorMessage) = ValidateInput(userName);
                if (!success)
                {
                    _logger.LogWarning(errorMessage);
                    return BadRequest(errorMessage);
                }

                return Ok(_statisticsService.HowLongDidLastChallengeTake(userName));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.SomethingWentWrongWithStatistics);
                return StatusCode((int)HttpStatusCode.InternalServerError, ErrorMessages.SomethingWentWrongWithStatistics);
            }
        }

        /// <summary>
        /// Gets the time length of how long it takes on average to identify the subject.
        /// </summary>
        /// <returns>Average time duration for all past challenges between the creation of the challenge and marking it solved. It returns null when no solved challenges were found.</returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult<TimeSpan?> AverageTimeToSolveChallenge()
        {
            try
            {
                return Ok(_statisticsService.AverageTimeToSolveChallenge());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.SomethingWentWrongWithStatistics);
                return StatusCode((int)HttpStatusCode.InternalServerError, ErrorMessages.SomethingWentWrongWithStatistics);
            }
        }

        /// <summary>
        /// Gets the time length of how long it takes on average for a person to identify the subject.
        /// </summary>
        /// <param name="userName">Username of the user that solved some challenges in the past.</param>
        /// <returns>Average time duration for all past challenges of a specific user between the creation of the challenge and marking it solved. It returns null when no solved challenges were found.</returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult<TimeSpan?> AverageTimeUserSolvesChallenge(string userName)
        {
            try
            {
                var (success, errorMessage) = ValidateInput(userName);
                if (!success)
                {
                    _logger.LogWarning(errorMessage);
                    return BadRequest(errorMessage);
                }

                return Ok(_statisticsService.AverageTimeToSolveChallenge(userName));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.SomethingWentWrongWithStatistics);
                return StatusCode((int)HttpStatusCode.InternalServerError, ErrorMessages.SomethingWentWrongWithStatistics);
            }
        }

        private (bool, string) ValidateInput(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return (false, string.Format(ErrorMessages.ValueIsEmpty, nameof(userName)));

            return (true, null);
        }
    }
}