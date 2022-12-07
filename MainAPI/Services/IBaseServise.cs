using MainAPI.Models;

namespace MainAPI.Services
{
    public interface IBaseServise:IDisposable
    {
        ResponseDTO responseModel { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
