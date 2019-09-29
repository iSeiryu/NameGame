using Microsoft.AspNetCore.Mvc;
using NameGame.Domain.Services.Interfaces;
using System.Net;

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

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public void NameToFaces()
        {

        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public void NameToFaces(string answer)
        {

        }
    }
}
