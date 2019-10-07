using NameGame.Service.Models;
using NameGame.Service.Models.Dto;
using System.Threading.Tasks;

namespace NameGame.Service.Services.Interfaces
{
    public interface IGameService
    {
        Task<NameToFacesChallenge> CreateNameToFacesChallengeAsync(ChallengeRequest request);
        Task<FaceToNamesChallenge> CreateFaceToNamesChallengeAsync(ChallengeRequest request);
        Task<ChallengeAnswerValidationResult> IsAnswerValidAsync(ChallengeAnswer answer);
    }
}
