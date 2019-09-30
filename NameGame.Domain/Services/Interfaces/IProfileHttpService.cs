using System.Threading.Tasks;

namespace NameGame.Domain.Services.Interfaces
{
    public interface IProfileHttpService
    {
        Task<T> Get<T>(string uri);
    }
}
