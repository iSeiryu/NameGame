using Microsoft.EntityFrameworkCore;
using NameGame.Persistence.DbContexts;
using NameGame.Persistence.Models;
using NameGame.Persistence.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace NameGame.Persistence.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly NameGameContext _context;

        public GameRepository(NameGameContext context)
        {
            _context = context;
        }

        public async Task<int> CreateChallenge(string correctAnswer, int userId)
        {
            var challenge = new Challenge(userId, correctAnswer);
            _context.Challenges.Add(challenge);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return challenge.Id;
        }

        public async Task<Challenge> GetChallenge(int challengeId)
        {
            return await _context.Challenges.FirstOrDefaultAsync(x => x.Id == challengeId).ConfigureAwait(false);
        }

        public async Task UpdateChallenge(Challenge challenge)
        {
            challenge.UpdatedDate = DateTime.Now;
            _context.Challenges.Attach(challenge);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
