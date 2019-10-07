using NameGame.Persistence.Models;
using System.Threading.Tasks;

namespace NameGame.Persistence.Repositories.Interfaces
{
    public interface IGameRepository
    {
        Task<Challenge> GetChallenge(int challengeId);
        Task<int> CreateChallenge(string correctAnswer, int userId);
        Task UpdateChallenge(Challenge challenge);
    }
}
