using NameGame.Persistence.DbContexts;
using NameGame.Persistence.Models;
using NameGame.Persistence.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NameGame.Persistence.Repositories
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly NameGameContext _context;

        public StatisticsRepository(NameGameContext context)
        {
            _context = context;
        }

        public int GetLastAttempts(string userName)
        {
            return _context.Challenges
                .Where(x => x.User.Name == userName)
                .OrderByDescending(x => x.Id)
                .Select(x => x.Attempts)
                .FirstOrDefault();
        }

        public Challenge GetLastSuccessfulChallenge(string userName)
        {
            return _context.Challenges
                .Where(x => x.User.Name == userName && x.Solved)
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();
        }

        public TimeSpan? AverageTimeToSolveChallenge()
        {
            var challenges = _context.Challenges.Where(x => x.Solved).ToList();
            if (challenges.Count == 0) return null;

            return CalculateAverage(challenges);
        }

        public TimeSpan? AverageTimeToSolveChallenge(string userName)
        {
            var challenges = _context.Challenges.Where(x => x.Solved && x.User.Name == userName).ToList();
            if (challenges.Count == 0) return null;

            return CalculateAverage(challenges);
        }

        private TimeSpan? CalculateAverage(List<Challenge> challenges)
        {
            var average = (long)challenges
                .Select(x => x.UpdatedDate.Ticks - x.CreatedDate.Ticks)
                .Average();

            return new TimeSpan(average);
        }
    }
}
