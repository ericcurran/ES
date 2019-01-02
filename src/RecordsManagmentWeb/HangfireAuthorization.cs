using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace RecordsManagmentWeb
{
    internal class HangfireAuthorization: IDashboardAuthorizationFilter
    {
            public bool Authorize([NotNull] DashboardContext context)
            {
                var httpcontext = context.GetHttpContext();
                return httpcontext.User.Identity.IsAuthenticated;
            }
       
    }
}