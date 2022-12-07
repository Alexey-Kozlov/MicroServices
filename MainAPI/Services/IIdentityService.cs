namespace MainAPI.Services
{
    public interface IIdentityService : IBaseServise
    {
        Task<T> GetIdentity<T>(string token);
    }
}
