using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MySql.BLL.Service;

namespace NCCFOhaukwu.Web.Controllers
{
    public class NewHereController : BaseController
    {
        public NewHereController(IService service)
            : base(service)
        {
        }
        
        //[OutputCache(Duration = 3600, VaryByParam = "*")]
        public async Task<ActionResult> Index()
        {
            var newhere = await Service.NewHere.GetAsync();
            return View(newhere.FirstOrDefault());
        }
    }
}