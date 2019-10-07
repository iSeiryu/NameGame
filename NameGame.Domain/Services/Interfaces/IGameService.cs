using NameGame.Domain.Models;
using NameGame.Domain.Models.Dto;
using System.Threading.Tasks;

namespace NameGame.Domain.Services.Interfaces
{
    public interface IGameService
    {
        Task<Challenge> CreateNameToFacesChallengeAsync(ChallengeRequest request);
        Task<bool> IsAnswerValidAsync(ChallengeAnswer answer);
    }
}
