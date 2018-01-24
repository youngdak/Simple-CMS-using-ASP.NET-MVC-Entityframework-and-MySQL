using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using DAL;
using NCCFOhaukwu.Web.Models;

namespace NCCFOhaukwu.Web
{
    public class Access : AuthorizeAttribute
    {
        private readonly Operations _operations;
        private readonly int _resourceId;
        private ResourceService _resourceService = new ResourceService();

        public Access(int resourceId, Operations operations)
        {
            _resourceId = resourceId;
            _operations = operations;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var principal = (ClaimsPrincipal) Thread.CurrentPrincipal;

            if (!principal.Identity.IsAuthenticated)
            {
                return false;
            }

            var roles = principal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray();
            return ResourceService.Authorize(_resourceId, _operations, roles);
        }
    }
}