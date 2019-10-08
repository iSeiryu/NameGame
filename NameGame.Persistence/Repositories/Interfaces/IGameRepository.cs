using NameGame.Persistence.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NameGame.Persistence.Repositories.Interfaces
{
    public interface IGameRepository
    {
        Task<Challenge> GetChallenge(int challengeId);
        Task<List<Challenge>> GetChallenges();
        Task<int> CreateChallenge(string correctAnswer, int userId);
        Task UpdateChallenge(Challenge challenge);
        Task<bool> DeleteChallenge(Challenge challenge);
    }
}
