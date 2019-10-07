using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NameGame.API.Constants;
using NameGame.Service.Services.Interfaces;
using System;
using System.Net;

namespace NameGame.API.Controllers
{
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

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult<int> HowLongDidLastChallengeTake(string userName)
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

        private (bool, string) ValidateInput(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return (false, string.Format(ErrorMessages.ValueIsEmpty, nameof(userName)));

            return (true, null);
        }
    }
}