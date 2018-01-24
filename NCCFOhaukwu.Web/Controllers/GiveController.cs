using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MySql.BLL.Service;

namespace NCCFOhaukwu.Web.Controllers
{
    public class GiveController : BaseController
    {
        public GiveController(IService service)
            : base(service)
        {
        }

        //[OutputCache(Duration = 3600, VaryByParam = "*")]
        public async Task<ActionResult> Index()
        {
            var give = await Service.Give.GetAsync();
            return View(give.FirstOrDefault());
        }
    }
}