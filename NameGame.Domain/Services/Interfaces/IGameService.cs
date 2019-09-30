using NameGame.Domain.Models;
using System.Threading.Tasks;

namespace NameGame.Domain.Services.Interfaces
{
    public interface IGameService
    {
        Task<Challenge> CreateChallenge(ChallengeRequest request);
        bool IsAnswerValid(ChallengeAnswer answer);
    }
}
