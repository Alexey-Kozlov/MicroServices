namespace Identity.Services
{
    public interface IBrokerService
    {
        Task SendToLog<T>(T logObject, string action, string token);
    }
}
