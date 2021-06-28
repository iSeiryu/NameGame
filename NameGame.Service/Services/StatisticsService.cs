using System;
using NameGame.Persistence.Repositories.Interfaces;
using NameGame.Service.Services.Interfaces;

namespace NameGame.Service.Services {
    public class StatisticsService : IStatisticsService {
        private readonly IStatisticsRepository _statisticsRepo;

        public StatisticsService(IStatisticsRepository statisticsRepo) {
            _statisticsRepo = statisticsRepo;
        }

        public int GetLastAttempts(string userName) {
            return _statisticsRepo.GetLastAttempts(userName);
        }

        public TimeSpan? HowLongDidLastChallengeTake(string userName) {
            var challenge = _statisticsRepo.GetLastSuccessfulChallenge(userName);
            if (challenge == null) return null;

            return challenge.UpdatedDate - challenge.CreatedDate;
        }

        public TimeSpan? AverageTimeToSolveChallenge() {
            return _statisticsRepo.AverageTimeToSolveChallenge();
        }

        public TimeSpan? AverageTimeToSolveChallenge(string userName) {
            return _statisticsRepo.AverageTimeToSolveChallenge(userName);
        }
    }
}
