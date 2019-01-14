using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Config
{
    public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            //TODO: try get IAuthorizationService
            // and use policy check instead of role check

            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            return httpContext.User.IsInRole("Administrators");
            //return httpContext.User.Identity.IsAuthenticated;
        }
    }
}
