using Microsoft.EntityFrameworkCore;
using NameGame.Persistence.DbContexts;
using NameGame.Persistence.Models;
using NameGame.Persistence.Repositories.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace NameGame.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly NameGameContext _context;

        public UserRepository(NameGameContext context)
        {
            _context = context;
        }

        public async Task<int> GetOrCreateUser(string userName)
        {
            var userId = await _context.Users
                .Where(x => x.Name == userName)
                .Select(x => x.Id)
                .FirstOrDefaultAsync().ConfigureAwait(false);

            if (userId < 1)
            {
                var user = new User() { Name = userName };
                _context.Users.Add(user);
                await _context.SaveChangesAsync().ConfigureAwait(false);

                userId = user.Id;
            }

            return userId;
        }
    }
}
