using System.Threading.Tasks;

namespace NameGame.Persistence.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<int> GetOrCreateUser(string userName);
    }
}
