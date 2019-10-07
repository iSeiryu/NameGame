using NameGame.Persistence.Models;
using System;

namespace NameGame.Persistence.Repositories.Interfaces
{
    public interface IStatisticsRepository
    {
        int GetLastAttempts(string userName);
        Challenge GetLastSuccessfulChallenge(string userName);
        TimeSpan? AverageTimeToSolveChallenge();
        TimeSpan? AverageTimeToSolveChallenge(string userName);
    }
}
