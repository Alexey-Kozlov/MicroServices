using Microsoft.Extensions.Options;

namespace MainAPI.Core
{
    public static class CoreServiceProvider
    {
        public static IServiceProvider Provider { get; set; }

        public static IOptions<OrdersPageSettings> GetActivityPageSettings()
        {
            return (IOptions<OrdersPageSettings>)Provider.GetService(typeof(IOptions<OrdersPageSettings>))!;
        }

        public static IOptions<RolesPageSetting> GetRolesPageSettings()
        {
            return (IOptions<RolesPageSetting>)Provider.GetService(typeof(IOptions<RolesPageSetting>))!;
        }
    }

    public class OrdersPageSettings : PageSettings
    {

    }

    public class RolesPageSetting : PageSettings
    {

    }

    public class PageSettings
    {
        public int PageSize { get; set; }
    }
}
