using NameGame.Persistence.Models;
using NameGame.Service.Models;
using NameGame.Service.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NameGame.Service.Services.Interfaces
{
    public interface IGameResourceService
    {
        Task<Challenge> GetChallenge(int id);
        Task<List<Challenge>> GetChallenges();
        Task<NameToFacesChallenge> CreateNameToFacesChallenge(ChallengeRequest request);
        Task<FaceToNamesChallenge> CreateFaceToNamesChallenge(ChallengeRequest request);
        Task<ChallengeAnswerValidationResult> IsAnswerValid(ChallengeAnswer answer);
        Task<bool> DeleteChallenge(Challenge challenge);
    }
}
