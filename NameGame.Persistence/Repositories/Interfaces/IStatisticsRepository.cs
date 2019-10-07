using NameGame.Persistence.Models;

namespace NameGame.Persistence.Repositories.Interfaces
{
    public interface IStatisticsRepository
    {
        int GetLastAttempts(string userName);
        Challenge GetLastSuccessfulChallenge(string userName);
    }
}
