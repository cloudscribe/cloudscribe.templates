using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    [ApiController]
    public class CsApiController : ControllerBase
    {
        private readonly IUserQueries _userQueries;
        private readonly SiteManager _siteManager;

        public CsApiController(
            IUserQueries userQueries, SiteManager siteManager)
        {
            _userQueries = userQueries;
            _siteManager = siteManager;
        }

        [Authorize(Roles = "Administrators")]
        [HttpGet, Route("csapi/GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userQueries.GetPage(
                _siteManager.CurrentSite.Id,
                1,
                100,
                "",
                0);

            // intentional delay so you get to see the Loading... indicator
            System.Threading.Thread.Sleep(3000);

            return new JsonResult(users.Data);
        }
    }
}
