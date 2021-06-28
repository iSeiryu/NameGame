using System.Threading.Tasks;

namespace NameGame.Service.Services.Interfaces {
    public interface IProfileHttpService {
        Task<T> Get<T>(string uri);
    }
}
