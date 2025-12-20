using System.Threading.Tasks;

namespace backend.Services;

public interface IRconService
{
    Task<bool> ExecuteCommandsAsync(string ip, int port, string password, params string[] commands);
}
