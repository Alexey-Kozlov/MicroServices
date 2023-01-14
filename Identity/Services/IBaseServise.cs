using Identity.Models;
using Models;

namespace Services
{
    public interface IBaseServise:IDisposable
    {
        ResponseDTO responseModel { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
