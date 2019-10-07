using NameGame.Persistence.DbContexts;
using NameGame.Persistence.Models;
using NameGame.Persistence.Repositories.Interfaces;
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
    }
}
