using NameGame.Service.Models;
using NameGame.Service.Models.Dto;
using System.Threading.Tasks;

namespace NameGame.Service.Services.Interfaces
{
    public interface IGameService
    {
        Task<Challenge> CreateNameToFacesChallengeAsync(ChallengeRequest request);
        Task<bool> IsAnswerValidAsync(ChallengeAnswer answer);
    }
}
