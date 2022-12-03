namespace Identity.Services
{
    public interface IUserAccessor
    {
        string GetUserName();
        string GetUserId();
    }
}
